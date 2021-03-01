using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ESUIManager : MonoBehaviour
{
    public static ESUIManager _instance;
    public Image enemyHpBar;
    public TextMeshProUGUI enemyHpBarText;
    public float floatingTextDestroyTime = 1.1f;
    public GameObject floatingText;

    public Button mergeSceneReturnButton;

    void Awake()
    {
        _instance = this;
    }

    public void ControlEnemyHpBar(float _currentHp, float _maxHp)
    {
        float fillValue = (_currentHp / _maxHp);
        enemyHpBar.fillAmount = fillValue;
        enemyHpBarText.text = _currentHp.ToString();

    } // ControlEnemyHpBar()

    public void ActivateEnemyHpBar()
    {
        enemyHpBar.transform.parent.gameObject.SetActive(true);
        enemyHpBarText.enabled = true;

    } // ActivateEnemyHpBar()

    public void DisableEnemyHpBar()
    {
        enemyHpBar.transform.parent.gameObject.SetActive(false);
        enemyHpBarText.enabled = false;

    } // DisableEnemyHpBar()
    public void SpawnFloatingText(string _text, Color _color, Transform _transform)
    {
        GameObject tmpFloatingText = Instantiate(floatingText, _transform.position, Quaternion.identity);
        tmpFloatingText.GetComponent<TextMeshPro>().text = _text;
        tmpFloatingText.GetComponent<TextMeshPro>().autoSizeTextContainer = true;
        tmpFloatingText.GetComponent<TextMeshPro>().color = _color;
        Destroy(tmpFloatingText, floatingTextDestroyTime);

    } // SpawnFloatingText()

    public void MergeSceneReturnButton()
    {
        PlayerPrefs.SetString("CurrentScene", "MergingScene");

        SceneManager.LoadScene("MergingScene");

    } // MergeSceneReturnButton()

} // class
