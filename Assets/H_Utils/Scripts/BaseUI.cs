using H_Utils;
using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    public abstract UIType Type { get; }
    public virtual void Show() => gameObject.SetActive(true);   
    public virtual void Hide() => gameObject.SetActive(false);  

}
