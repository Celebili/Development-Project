using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
/// <summary>
/// 跟随宝可梦
/// </summary>
public class Follow : MonoBehaviour, Interactable
{
    public bool isRes;
    public string path;
    [SerializeField] Transform _trans;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Dialog dialog;
    private List<Sprite> walkUpSprite = new List<Sprite>();
    private List<Sprite> walkDownSprite = new List<Sprite>();
    private List<Sprite> walkLeftSprite = new List<Sprite>();
    private List<Sprite> walkRightSprite = new List<Sprite>();
    public float moveSpeed;
    /// <summary>
    /// 玩家上一次的X位移,follow的下次位移
    /// </summary>
    private float x = 0f;
    /// <summary>
    /// 玩家上一次的Y位移,follow的下次位移
    /// </summary>
    private float y = -1f;
    SpriteAnimator walkUpAnim;
    SpriteAnimator walkDownAnim;
    SpriteAnimator walkLeftAnim;
    SpriteAnimator walkRightAnim;
    SpriteAnimator currentAnim;

    bool isActive;
    private int currentID = 25;

    private void Start()
    {
        if(isRes)
        {
            SetNpcSprite(path);
        }
        walkUpAnim    = new SpriteAnimator(walkUpSprite, spriteRenderer);
        walkDownAnim  = new SpriteAnimator(walkDownSprite, spriteRenderer);
        walkLeftAnim  = new SpriteAnimator(walkLeftSprite, spriteRenderer);
        walkRightAnim = new SpriteAnimator(walkRightSprite, spriteRenderer);
        currentAnim   = walkDownAnim;
        isActive      = true;
        //ResetOriginPosition();
    }

    public void SetPosition(Vector3 targetPos)
    {
        _trans.position = targetPos;
        //ResetOriginPosition();
    }

    /// <summary>
    /// 根据玩家Input计算follow移动
    /// </summary>
    /// <param name="inputX"></param>
    /// <param name="inputY"></param>
    /// <returns>Vector2 move</returns>
    public async void MoveHandler(float _x, float _y)
    {
        Vector3 targetPos = _trans.position;
        targetPos.x += x;
        targetPos.y += y;

        if      (_x > 0f && x !=  1f)
        {
            x =  1f; y =  0f; currentAnim = walkRightAnim;
        }
        else if (_x < 0f && x != -1f)
        {
            x = -1f; y =  0f; currentAnim = walkLeftAnim;
        }
        else if (_y > 0f && y !=  1f)
        {
            x =  0f; y =  1f; currentAnim = walkUpAnim;
        }
        else if (_y < 0f && y != -1f)
        {
            x =  0f; y = -1f; currentAnim = walkDownAnim;
        }

        while((targetPos - _trans.position).sqrMagnitude > Mathf.Epsilon)
        {
            _trans.position =
	    	    Vector3.MoveTowards(_trans.position, targetPos, moveSpeed * Time.deltaTime);
            await UniTask.Yield();
        }
        _trans.position = targetPos;
    }

    private void Update()
    {
        //if(!isMoving && moveQueue.Count > 0)
        //{
        //    Moving();
        //}
        currentAnim.HandleUpdate();
    }
#region 队列移动
    //做完了(还可以调整些细节)
    //这个方法可以完全防止因帧率导致宝可梦位置错误, 但如果宝可梦能跟上就不用

    //方法
    //把本脚本Awake、Update、SetPosition注释的取消,
    //PlayerMovement里 Move()中 follow.FollowMoveQueue(MoveX, MoveY);替换掉MoveHandler
    //GameManager里LoadScene()中 await UniTask.WaitUntil(() => Player.FollowPokemon.EndOfTheMoveQueue == true);注释取消

    //隐藏bug Npc如果也有跟随宝可梦, 在没有完成移动的情况下改变位置也可能会出错

    /*public void ResetOriginPosition() => originPosition = _trans.position;
    public bool EndOfTheMoveQueue => !isMoving && moveQueue.Count == 0;
    Queue<Vector3> moveQueue = new Queue<Vector3>(); //Enqueue加 Dequeue删
    Queue<SpriteAnimator> spriteAnimatorQueue = new Queue<SpriteAnimator>();
    Vector3 originPosition;
    public void FollowMoveQueue(float _x, float _y)
    {
        Vector3 targetPos = originPosition;
        targetPos.x += x;
        targetPos.y += y;
        originPosition = targetPos;
        moveQueue.Enqueue(targetPos);
        if      (_x > 0f && x !=  1f)
        {
            x =  1f; y =  0f; spriteAnimatorQueue.Enqueue(walkRightAnim);
        }
        else if (_x < 0f && x != -1f)
        {
            x = -1f; y =  0f; spriteAnimatorQueue.Enqueue(walkLeftAnim);
        }
        else if (_y > 0f && y !=  1f)
        {
            x =  0f; y =  1f; spriteAnimatorQueue.Enqueue(walkUpAnim);
        }
        else if (_y < 0f && y != -1f)
        {
            x =  0f; y = -1f; spriteAnimatorQueue.Enqueue(walkDownAnim);
        }
        else
        {
            spriteAnimatorQueue.Enqueue(currentAnim);
        }
    }
    private bool isMoving = false;
    private async void Moving()
    {
        isMoving = true;
        Vector3 targetPos = moveQueue.Dequeue();
        currentAnim = spriteAnimatorQueue.Dequeue();
        while((targetPos - _trans.position).sqrMagnitude > Mathf.Epsilon)
        {
            _trans.position =
	    	    Vector3.MoveTowards(_trans.position, targetPos, moveSpeed * Time.deltaTime);
            await UniTask.Yield();
        }
        _trans.position = targetPos;
        await UniTask.Yield();
        isMoving = false;
    }*/
#endregion

    /// <summary>
    /// 关掉follow
    /// </summary>
    public void OnClose()
    {
        isActive = false;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 重置Pokemon
    /// </summary>
    /// <param name="pokeID"></param>
    /// <param name="isShiny"></param>
    /// <param name="pos"></param>
    public void ResetPokemon(int pokeID, bool isShiny, Vector3 pos)
    {
        if(currentID == pokeID)
        {
            return;
        }

        /*if(x != 0)
        {
            pos.x += x > 0? -1 : 1;
            pos.y -= 0.09f;
        }
        else
        {
            if(y > 0)
            {
                pos.y -= 1.09f;
            }
            else
            {
                pos.y += 0.91f;
            }
        }*/

        walkUpSprite.Clear();
        walkDownSprite.Clear();
        walkLeftSprite.Clear();
        walkRightSprite.Clear();
        currentID = pokeID;
        SetNpcSprite($"Pokemon/Follow/{pokeID}f/");

        if(!isActive)
        {
            gameObject.SetActive(true);
            SetPosition(pos);
        }
    }

    public void SetNpcSprite(string loadPath)
    {
        Sprite[] sprites = ResM.Instance.LoadAllSprites(loadPath);
        if(sprites == null || sprites.Length == 0)
        {
            currentID = 54;
            sprites = ResM.Instance.LoadAllSprites("Pokemon/Follow/54f/");
        }

        if(sprites.Length < 9)
        {
            walkUpSprite.Add(sprites[1]);
            walkUpSprite.Add(sprites[0]);

            walkDownSprite.Add(sprites[3]);
            walkDownSprite.Add(sprites[2]);

            walkLeftSprite.Add(sprites[5]);
            walkLeftSprite.Add(sprites[4]);

            walkRightSprite.Add(sprites[7]);
            walkRightSprite.Add(sprites[6]);
        }
        else
        {
            walkUpSprite.Add(sprites[1]);
            walkUpSprite.Add(sprites[0]);
            walkUpSprite.Add(sprites[2]);
            walkUpSprite.Add(sprites[0]);

            walkDownSprite.Add(sprites[4]);
            walkDownSprite.Add(sprites[3]);
            walkDownSprite.Add(sprites[5]);
            walkDownSprite.Add(sprites[3]);

            walkLeftSprite.Add(sprites[7]);
            walkLeftSprite.Add(sprites[6]);
            walkLeftSprite.Add(sprites[8]);
            walkLeftSprite.Add(sprites[6]);

            walkRightSprite.Add(sprites[10]);
            walkRightSprite.Add(sprites[9]);
            walkRightSprite.Add(sprites[11]);
            walkRightSprite.Add(sprites[9]);
        }
    }

    public void Interact(Transform initiator)
    {
        if(DialogManager.Instance.Free)
        {
            DialogManager.Instance.Info
            (
                dialog, AllPokemon.GetPokemonByID(currentID).Name, null
            );
        }
        else
        {
            DialogManager.Instance.Typing();
        }
    }

#region 存储和读取数据
    public FollowSaveData CaptureState()
	{
        Vector3 currentPos = _trans.position;
        FollowSaveData saveData = new FollowSaveData()
        {
            pokemonID = currentID,
            x = currentPos.x,
            y = currentPos.y,
            z = currentPos.z,
            nextX = x,
            nextY = y,
            isActive = isActive
        };
		return saveData;
	}

	public void RestoreState(FollowSaveData saveData)
	{
		_trans.position = new Vector3(saveData.x, saveData.y, saveData.z);
        x = saveData.nextX; y = saveData.nextY;
        if(isActive)
        {
            currentID = saveData.pokemonID;
            //就开follow，false就关了
        }
	}
    #endregion
}

[System.Serializable]
public struct FollowSaveData
{
    public int pokemonID;
    public float x, y, z;
    public float nextX;//follow的下次位移，玩家的上次位移
    public float nextY;
    public bool isActive;
}