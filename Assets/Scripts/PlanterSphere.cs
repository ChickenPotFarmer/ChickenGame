using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanterSphere : MonoBehaviour
{
    public bool selectionActive;
    public Planter planterMaster;
    public WeedPlant selectedPlant;

    //private void Update()
    //{
    //    if (selectedPlant != null && !selectedPlant.selected && !selectedPlant.isPlanted)
    //    {
    //        if (Vector3.Distance(selectedPlant.transform.position, transform.position) > 2.5f)
    //        {
    //            selectedPlant.SetNone();
    //            selectedPlant = null;
    //            planterMaster.selectedPlant = null;
    //        }
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Weed Plant"))
    //    {
    //        // add if statement for if the plant has been planted
    //        WeedPlant foundPlant = collision.gameObject.GetComponent<WeedPlant>();

    //        if (!foundPlant.isPlanted)
    //        {
    //            if (selectedPlant != null)
    //            {
    //                if (foundPlant != selectedPlant)
    //                {
    //                    if (!selectedPlant.isPlanted)
    //                        selectedPlant.SetNone();

    //                    selectedPlant = foundPlant;
    //                }
    //            }
    //            else
    //                selectedPlant = collision.gameObject.GetComponent<WeedPlant>();

    //            planterMaster.selectedPlant = selectedPlant;

    //            selectedPlant.SetFullGrown();
    //        }
    //    }
    //}

    public void MoveSphere()
    {
        if (!selectionActive)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    transform.position = hit.point;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x);
    }
}
