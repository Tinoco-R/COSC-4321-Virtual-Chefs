using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SphereInteraction : MonoBehaviour
{
    private GameObject ticket; // Reference to the ticket object
    private bool isTicketGrabbed = false; // Flag to check if the ticket is grabbed

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entered object has the "Ticket" tag
        if (other.tag == "Ticket")
        {
            ticket = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset the ticket reference when it exits the sphere
        if (other.tag == "Ticket")
        {
            ticket = null;
        }
    }

    private void Update()
    {
        // If the ticket is grabbed and released within the sphere, place it at the sphere's position
        if (ticket != null && !isTicketGrabbed && IsInsideSphere(ticket.transform.position))
        {
            PlaceTicket();
        }
    }

    private bool IsInsideSphere(Vector3 position)
    {
        // Check if the position is inside the sphere using distance comparison
        float distance = Vector3.Distance(position, transform.position);
        return distance <= transform.localScale.x / 2; // Assuming the sphere is a perfect sphere, comparing with its radius
    }

    private void PlaceTicket()
    {
        ticket.transform.position = transform.position;
        ticket = null; // Reset the ticket reference after placing it
    }

    public void GrabTicket()
    {
        isTicketGrabbed = true;
    }

    public void ReleaseTicket()
    {
        isTicketGrabbed = false;
    }
}
