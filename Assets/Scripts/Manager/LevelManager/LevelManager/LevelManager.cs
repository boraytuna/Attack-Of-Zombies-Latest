using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] private GameObject transitionPanelCanvas;
    [SerializeField] private Image progressBar;

    private float target;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        transitionPanelCanvas.SetActive(false);
    }

    public async void LoadScene(int sceneIndex)
    {
        target = 0;
        progressBar.fillAmount = 0;
        
        var scene = SceneManager.LoadSceneAsync(sceneIndex);
        scene.allowSceneActivation = false;

        transitionPanelCanvas.SetActive(true);
        do{
            await Task.Delay(1);
            target = scene.progress;
        }while(scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        transitionPanelCanvas.SetActive(false);
    }
    void Update()
    {
        progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, target, 3 * Time.deltaTime);
    }
 }
