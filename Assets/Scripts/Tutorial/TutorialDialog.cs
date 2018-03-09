using UnityEngine;
using UnityEngine.UI;

public class TutorialDialog : MonoBehaviour {

    public Button previousSlide;
    public Button nextSlide;
    public Text text;
    public Text page;

    public string[] slides;

    int currentSlide = 0;

    private void Awake()
    {
        RefreshSlide();
    }

    public void NextSlide()
    {
        currentSlide = Mathf.Clamp(currentSlide + 1, 0, slides.Length - 1);
        RefreshSlide();
    }

    public void PreviousSlide()
    {
        currentSlide = Mathf.Clamp(currentSlide - 1, 0, slides.Length - 1);
        RefreshSlide();
    }

    public void RefreshSlide()
    {
        page.text = (currentSlide + 1) + "/" + slides.Length;
        text.text = slides[currentSlide];
        nextSlide.interactable = currentSlide != slides.Length - 1;
        previousSlide.interactable = currentSlide != 0;
    }

    public void SetSlides(string[] _slides)
    {
        slides = _slides;
        currentSlide = 0;
        RefreshSlide();
    }

}
