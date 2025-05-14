// using System.Collections.Generic;
// using System.Linq;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;

// public class TargetHandler : MonoBehaviour 
// {
//     [SerializeField] private NavigationController navigationController;
//     [SerializeField] private TextAsset targetModelData;
//     [SerializeField] private TMP_InputField searchInputField;
//     [SerializeField] private Transform searchResultsParent;
//     [SerializeField] private GameObject searchResultButtonPrefab;

//     [SerializeField] private GameObject targetObjectPrefab;
//     [SerializeField] private Transform[] targetObjectsParentTransforms;

//     private List<TargetFacade> currentTargetItems = new List<TargetFacade>();
//     private List<GameObject> currentResultButtons = new List<GameObject>();

//     private void Start() 
//     {
//         GenerateTargetItems();
//         SetupSearchField();
//     }

//     private void SetupSearchField()
//     {
//         searchInputField.onValueChanged.AddListener(UpdateSearchResults);
//         searchInputField.onSubmit.AddListener(SelectFirstResult);
//     }

//     private void GenerateTargetItems() 
//     {
//         IEnumerable<Target> targets = GenerateTargetDataFromSource();
//         foreach (Target target in targets) 
//         {
//             currentTargetItems.Add(CreateTargetFacade(target));
//         }
//     }

//     private IEnumerable<Target> GenerateTargetDataFromSource() 
//     {
//         return JsonUtility.FromJson<TargetWrapper>(targetModelData.text).TargetList;
//     }

//     private TargetFacade CreateTargetFacade(Target target) 
//     {
//         GameObject targetObject = Instantiate(targetObjectPrefab, 
//             targetObjectsParentTransforms[target.FloorNumber], 
//             false);

//         targetObject.SetActive(true);
//         targetObject.name = $"{target.FloorNumber} - {target.Name}";
//         targetObject.transform.localPosition = target.Position;
//         targetObject.transform.localRotation = Quaternion.Euler(target.Rotation);

//         TargetFacade targetData = targetObject.GetComponent<TargetFacade>();
//         targetData.Name = target.Name;
//         targetData.FloorNumber = target.FloorNumber;

//         return targetData;
//     }

//     private void UpdateSearchResults(string searchText)
//     {
//         ClearResults();
        
//         if(string.IsNullOrWhiteSpace(searchText)) return;

//         var filteredTargets = currentTargetItems
//             .Where(t => t.Name.ToLower().Contains(searchText.ToLower()))
//             .ToList();

//         foreach(var target in filteredTargets)
//         {
//             GameObject button = Instantiate(searchResultButtonPrefab, searchResultsParent);
//             currentResultButtons.Add(button);
            
//             button.GetComponentInChildren<TMP_Text>().text = $"{target.FloorNumber} - {target.Name}";
//             button.GetComponent<Button>().onClick.AddListener(() => 
//             {
//                 SetSelectedTargetPosition(target);
//                 searchInputField.text = $"{target.FloorNumber} - {target.Name}";
//                 ClearResults();
//             });
//         }
//     }

//     private void SelectFirstResult(string searchText)
//     {
//         if(currentResultButtons.Count > 0)
//         {
//             currentResultButtons[0].GetComponent<Button>().onClick.Invoke();
//         }
//     }

//     private void ClearResults()
//     {
//         foreach(GameObject button in currentResultButtons)
//         {
//             Destroy(button);
//         }
//         currentResultButtons.Clear();
//     }

//     public void SetSelectedTargetPosition(TargetFacade target)
//     {
//         navigationController.TargetPosition = target.transform.position;
//     }

//     public TargetFacade GetCurrentTargetByTargetText(string targetText) 
//     {
//         return currentTargetItems.Find(x =>
//             x.Name.ToLower().Equals(targetText.ToLower()));
//     }
// }