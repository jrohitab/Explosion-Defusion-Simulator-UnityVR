using UnityEngine;

public class PageSwitcher : MonoBehaviour
{
    public GameObject[] pages; // Array to hold pages
    private int currentPage = 0;

    void Start()
    {
        ShowPage(currentPage);
    }

    public void NextPage()
    {
        Debug.Log("Next Page function Entered");
        if (currentPage < pages.Length - 1)
        {
            Debug.Log("Next Page IF Entered");
            
            currentPage++;
            ShowPage(currentPage);
        }
    }

    public void PreviousPage()
    {
        Debug.Log("Prev Page function Entered");
        if (currentPage > 0)
        {
            Debug.Log("Prev Page IF Entered");
            currentPage--;
            ShowPage(currentPage);
        }
    }

    private void ShowPage(int pageIndex)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == pageIndex);
        }
    }
}
