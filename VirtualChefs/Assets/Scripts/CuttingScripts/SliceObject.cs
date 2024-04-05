using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEditorInternal;

public struct SliceShapeSize
{
    public string Name;
    public string BlockName;
    public Vector3 Size;
}

public static class SliceShapeManager
{
    public static SliceShapeSize[] SliceShapeSizes = new SliceShapeSize[]
    {
        new() { Name = "CheeseSlice", BlockName = "CheeseBlock", Size = new Vector3(.19f, .1f, .11f) },
        new() { Name = "LettuceSlice", BlockName = "LettuceBlock",Size = new Vector3(.15f, .1f, .18f) },
        new() { Name = "TomatoSlice", BlockName = "TomatoBlock", Size = new Vector3(.15f, .1f, .15f) }
    };
}

public class SliceObject : MonoBehaviour
{
    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public VelocityEstimator velocityEstimator;
    public LayerMask sliceableLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer);
        // Testing
        if (hasHit)
        {
            GameObject target = hit.transform.gameObject;
            Slice(target);
        }
    }

    public Material GetMaterial(GameObject food)
    {
        Material material = null;
        string materialName = "";

        if (food.tag == "CheeseBlock")
        {
            materialName = "CheeseYellow";
        }
        else if (food.tag == "TomatoBlock")
        {
            materialName = "TomatoRed";
        }
        else if (food.tag == "LettuceBlock")
        {
            materialName = "LettuceGreen";
        }

        // Load the material from the Resources folder
        if (!string.IsNullOrEmpty(materialName))
        {
            material = Resources.Load<Material>("Materials/" + materialName);
        }

        return material;
    }

    // Used to help calculate volume of an object given its mesh
    float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;
        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }

    // Used to calculate the volume of an object given its mesh to determine if a sliced object prefab should spawn
    float VolumeOfMesh(Mesh mesh)
    {
        float volume = 0;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);
        }
        return Mathf.Abs(volume);
    }

    // Gets the volume of a game object used to see if a sliced object prefab should spawn
    public float CalculateVolume(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            Bounds bounds = renderer.bounds;
            Vector3 size = bounds.size;
            float volume = size.x * size.y * size.z;
            return volume;
        }
        else
        {
            Debug.LogError("Object does not have a Renderer component.");
            return 0f;
        }
    }

    // Gets the volume of a Vector3 used to see if a sliced object prefab should spawn
    public float CalculateVolume(Vector3 dimensions)
    {
        return dimensions.x * dimensions.y * dimensions.z;
    }

    // Converts a manually sliced object into its prefab after ensuring it is of a good slice
    public void ConvertToSlicePrefab(GameObject slicedObject, string sliceName)
    {
        string prefabPath = "Prefabs/Combine/" + sliceName;
        GameObject slicePrefab = Resources.Load<GameObject>(prefabPath);

        if (slicePrefab != null)
        {
            Instantiate(slicePrefab, slicedObject.transform.position, slicedObject.transform.rotation);
            Destroy(slicedObject);
        }
        else
        {
            Debug.LogError("Prefab not found: " + prefabPath);
        }
    }

    // Here the tolerance is defined to see if a cut object is "close enough" to sliced prefab version
    private bool IsCloseToSize(GameObject slice, string name)
    {
        MeshFilter meshFilter = slice.GetComponent<MeshFilter>();
        Mesh mesh;
        float volume = -1.0f;
        if (meshFilter != null)
        {
            mesh = meshFilter.mesh;
            volume = VolumeOfMesh(mesh);
        }

        float upperThreshold = 1.0f;
        float lowerThreshold = 0.0f;
        print(name);
        if (name == "CheeseSlice")
        {
            lowerThreshold = 0.000220f; //next was 0.00001130f; 1/3 of block
            upperThreshold = 0.000280f;     // Orig 0.00001215f; next was 0.00001300f;
        }
        if (name == "LettuceSlice")
        {
            lowerThreshold = 0.00001840f;
            upperThreshold = 0.00001940f; // Orig 0.00001890f;
        }
        if (name == "TomatoSlice")
        {
            lowerThreshold = 0.000300f;
            upperThreshold = 0.000475f;
        }

        print(lowerThreshold + " " + upperThreshold);

        if (volume <= upperThreshold && volume >= lowerThreshold)
        {
            return true;
        }
        return false;
    }

    /*
    // Here the tolerance is defined to see if a cut object is "close enough" to sliced prefab version
    private bool IsCloseToSize(Vector3 size, Vector3 targetSize, string name)
    {
        float volume = CalculateVolume(size);
        float targetVolume = CalculateVolume(targetSize);
        float difference = Mathf.Abs(volume - targetVolume);
        float upperThreshold = 1.0f;
        float lowerThreshold = 0.0f;
        print(name);
        if (name == "CheeseSlice")
        {
            lowerThreshold = 0.000084f; //next was 0.00001130f; 1/3 of block
            upperThreshold = 0.000120f;     // Orig 0.00001215f; next was 0.00001300f;
        }
        if (name == "LettuceSlice")
        {
            lowerThreshold = 0.00001840f;
            upperThreshold = 0.00001940f; // Orig 0.00001890f;
        }
        if (name == "TomatoSlice")
        {
            lowerThreshold = 0.000300f;
            upperThreshold = 0.000475f;
        }

        print("abs(" + volume + " - " + targetVolume + ") = " + difference);
        print(lowerThreshold + " " + upperThreshold);

        if (volume <= upperThreshold && volume >= lowerThreshold)
        {
            return true;
        }
        return false;
    }
    */

    // Checks to see size of cut and send object to be converted to its sliced version or not
    public void CheckAndConvertToSlicePrefab(GameObject slicedObject)
    {
        Bounds bounds = slicedObject.GetComponent<Renderer>().bounds;
        Vector3 size = bounds.size;
        string name = slicedObject.tag;
        foreach (var sliceShapeSize in SliceShapeManager.SliceShapeSizes)
        {
            //if (sliceShapeSize.BlockName == name && IsCloseToSize(size, sliceShapeSize.Size, sliceShapeSize.Name))
            if (sliceShapeSize.BlockName == name && IsCloseToSize(slicedObject, sliceShapeSize.Name))
            {
                ConvertToSlicePrefab(slicedObject, sliceShapeSize.Name);
                break;
            }
        }
    }

    // Slices an object into two different pieces
    public void Slice(GameObject target)
    {
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        planeNormal.Normalize();

        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal);

        if (hull != null)
        {
            // Gets material to fill for missing material after a cut
            Material targetMaterial = GetMaterial(target);

            // Splits the target into two parts
            GameObject upperHull = hull.CreateUpperHull(target, targetMaterial);
            SetupSlicedComponent(upperHull, target);

            GameObject lowerHull = hull.CreateLowerHull(target, targetMaterial);
            SetupSlicedComponent(lowerHull, target);

            // Destroys original target
            Destroy(target);

            // Checks to see if sliced prefab can spawn in place of a hull
            CheckAndConvertToSlicePrefab(upperHull);
            CheckAndConvertToSlicePrefab(lowerHull);
        }
    }

    // Sets up sliced objects to be sliceable themselves
    public void SetupSlicedComponent(GameObject slicedObject, GameObject target)
    {
        /*
        slicedObject.tag = target.tag;
        slicedObject.layer = LayerMask.NameToLayer("Sliceable");

        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        Grabbable grab = slicedObject.AddComponent<Grabbable>();
        HandGrabInteractable grabInteract = slicedObject.AddComponent<HandGrabInteractable>();
        ObjectGrabCheck grabCheck = slicedObject.AddComponent<ObjectGrabCheck>();

        collider.convex = true;
        //rb.AddExplosionForce(cutForce, slicedObject.transform.position, .2f);
        */

        slicedObject.tag = target.tag;
        slicedObject.layer = LayerMask.NameToLayer("Sliceable");

        // Ensure the GameObject has a Rigidbody component
        Rigidbody rb = slicedObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = slicedObject.AddComponent<Rigidbody>();
            // Configure Rigidbody as needed, for example:
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        // Add MeshCollider component to the slicedObject
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;

        // Add Grabbable component to the slicedObject
        Grabbable grab = slicedObject.AddComponent<Grabbable>();

        // Add HandGrabInteractable component to the slicedObject
        HandGrabInteractable grabInteract = slicedObject.AddComponent<HandGrabInteractable>();
        // Explicitly check if the Rigidbody is found
        if (grabInteract.Rigidbody == null)
        {
            print("Rigidbody not found on HandGrabInteractable.");
            grabInteract.InjectRigidbody(rb);
        }

        if (grabInteract.PointableElement == null)
        {
            print("Pointable element not found on HandGrabInteractable.");
            grabInteract.InjectOptionalPointableElement(grab);
        }

        // Add ObjectGrabCheck component to the slicedObject
        ObjectGrabCheck grabCheck = slicedObject.AddComponent<ObjectGrabCheck>();
    }
}
