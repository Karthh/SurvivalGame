using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MaterialNodes
{
    DISSOLVE = 0,
    GLOW_SPRITE = 1
}
public class EntityShaderManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Entity entity;
    float dissolveTimer = 1.0f;
    public List<Material> materialList;
    public Texture2D animationMap;
    public Texture2D emissionMap;

    public void InitializeShaderManager()
    {
        entity = GetComponent<Entity>();
        //materialList[0] = Resources.Load("Materials/Dissolve", typeof(Material)) as Material;
       // materialList[0] = Resources.Load("Materials/Glow_Emission", typeof(Material)) as Material;
    }
    public Material SetMaterial(MaterialNodes newMaterial, SpriteRenderer renderer)
    {
        Material mat = materialList[(int)newMaterial];
        renderer.material = mat;
        return mat;
    }
    public void SetMaterial(Material material, SpriteRenderer renderer)
    {
        renderer.material = material;
    }
    public Color SetColorIntensity(Color color, float intensity)
    {
        float factor = Mathf.Pow(2, intensity);
        Color retColor = new Color(color.r * intensity, color.g * intensity, color.b * intensity, color.a);
        return retColor;
    }
    /// <summary>
    /// Dissolves Enitity on Death
    /// </summary>
    /// <param name="lerpTime">time to lerp</param>
    /// <param name="waitTime">time before dissolve effect</param>
    /// <param name="fadeColor">the color to fade to</param>
    /// <returns></returns>
    public IEnumerator DissolveOnDeath(float lerpTime, float waitTime, Color fadeColor, SpriteRenderer renderer)
    {
        //if(renderer.material != materialList[(int)MaterialNodes.DISSOLVE]) { yield break; }
        entity.isDying = true;
        yield return new WaitForSeconds(waitTime);
        if (entity.isDying)
        {
            dissolveTimer -= Time.deltaTime / 2; //decrement the timer
            if (dissolveTimer <= 0) //done dissolving
            {
                dissolveTimer = 0;
                entity.isDying = false;
            }
            renderer.color = Color.Lerp(renderer.color, fadeColor, lerpTime * Time.deltaTime); //lerp the color of the entity to the fade color
            yield return new WaitForSeconds(0.5f);
            Dissolve(renderer); //use the dissolve shader
            yield return new WaitForSeconds(1f);
        }
        entity.isDead = true;
        Destroy(gameObject, 0.1f); //destroy the game object

    }
    /// <summary>
    /// Dissolve Shader Code
    /// </summary>
    private void Dissolve(SpriteRenderer renderer)
    {
        //renderer.material = materialList[(int)MaterialNodes.DISSOLVE];
        if (entity.isDying)
        {
            dissolveTimer -= Time.deltaTime;
            if (dissolveTimer <= 0)
            {
                dissolveTimer = 0;
                entity.isDying = false;
            }
            renderer.material.SetFloat("_Fade", dissolveTimer);
        }
    }
    public Material SetDissolveParams(Color color, float scale)
    {
        Material mat = materialList[(int)MaterialNodes.DISSOLVE];
        mat.SetColor("_OutlineColor", color);
        mat.SetFloat("_Scale", scale);
        return mat;
    }
    public Material SetGlowSpriteParams(Color color)
    {
        Material mat = materialList[(int)MaterialNodes.DISSOLVE];
        mat.SetTexture("_MainTex", animationMap);
        mat.SetTexture("_Emission", emissionMap);
        mat.SetColor("_Color", color);
        return mat;
;    }
}
