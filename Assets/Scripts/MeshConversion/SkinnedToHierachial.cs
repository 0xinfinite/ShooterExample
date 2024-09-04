using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

[ExecuteInEditMode()]
public class SkinnedToHierachial : MonoBehaviour
{
    //public struct HierachialMesh
    //{

    //}
    Dictionary<Transform, Dictionary<int, Dictionary<int, int>>> vertexDirecionaryForEachBone;   //bone, newVertexIndex, origVertexIndex, targetVertexIndex
    Dictionary<Transform, List<int[]>> dictForHierachialMeshes;

    public SkinnedMeshRenderer targetSkinned;

    // Start is called before the first frame update
    void OnEnable()
    {
        if(targetSkinned == null)
        {
            Debug.LogWarning("You have to assign skinned mesh renderer first");
            enabled = false;
            return;
        }
        ConvertToHirachialMesh();
        enabled = false;
    }

    void ConvertToHirachialMesh()
    {
        dictForHierachialMeshes = new Dictionary<Transform, List<int[]>>();

        var mesh = targetSkinned.sharedMesh;
        Debug.Log("Vertex count : "+mesh.vertexCount + " / All triangle count : "+mesh.triangles.Length);
        var bonesPerVertex = mesh.GetBonesPerVertex();
        var boneWeights = mesh.GetAllBoneWeights();

        var bones = targetSkinned.bones;

        vertexDirecionaryForEachBone = new Dictionary<Transform, Dictionary<int, Dictionary<int, int>>>();
        foreach (var bone in bones)
        {
            vertexDirecionaryForEachBone.Add(bone, new Dictionary<int, Dictionary<int, int>>());
        }

        int boneWeightIndex = 0;
        for (int i = 0; i<mesh.vertexCount;++i)
        {
            var vertex = mesh.vertices[i];
            var numberOfBonesForThisVertex = bonesPerVertex[i];
            for (int j = 0; j < numberOfBonesForThisVertex; j++)
            {
                var boneWeight = boneWeights[boneWeightIndex];
                if (boneWeight.weight > 0.9f)
                {
                    int vertexId = i;
                    Debug.Log("This bone has weight 1 of vertex : "+ bones[boneWeight.boneIndex].gameObject.name +"/"+i);
                    if (dictForHierachialMeshes.ContainsKey(bones[boneWeight.boneIndex]))
                    {
                        dictForHierachialMeshes[bones[boneWeight.boneIndex]].
                            Add(new int[3] { mesh.triangles[(vertexId) * 3 + 0], mesh.triangles[(vertexId) * 3 + 1], mesh.triangles[(vertexId) * 3 + 2] });
                    }
                    else
                    {
                        dictForHierachialMeshes.Add(bones[boneWeight.boneIndex], new List<int[]>()
                        { new int[3] { mesh.triangles[(vertexId) * 3 + 0], mesh.triangles[(vertexId) * 3 + 1], mesh.triangles[(vertexId) * 3 + 2] } }); 
                    }
                    //dictForHierachialMeshes[bones[boneWeight.boneIndex]].Add();
                }
                boneWeightIndex++;
            }
        }

        //for (int i = 0; i < bones.Length; ++i)
        //{
        //    var bone = bones[i];

        //    if (dictForHierachialMeshes.ContainsKey(bone))
        //    {
        //    }
        //}
        CreateHierachialMesh();
    }

    void CreateHierachialMesh()
    {
        Mesh origMesh = targetSkinned.sharedMesh;
        Mesh newMesh = new Mesh();

        Dictionary<Transform, List<Vector3>> newVertices = new Dictionary<Transform, List<Vector3>>();
        Dictionary<Transform, List<Vector3>> newNormals = new Dictionary<Transform, List<Vector3>>();
        Dictionary<Transform, List<Vector4>> newTangent = new Dictionary<Transform, List<Vector4>>();
        Dictionary<Transform, List<Color>> newColors = new Dictionary<Transform, List<Color>>();
        Dictionary<Transform, List<Vector2>> newUV = new Dictionary<Transform, List<Vector2>>();
        Dictionary<Transform, List<Vector2>> newUV2 = new Dictionary<Transform, List<Vector2>>();
        Dictionary<Transform, List<Vector2>> newUV3 = new Dictionary<Transform, List<Vector2>>();
        Dictionary<Transform, List<int>> newTriangles = new Dictionary<Transform, List<int>>();// { new Dictionary<Transform, List<int>()};
                                                                                               //Dictionary<Transform, List<Vector3> newVertices = new Dictionary<Transform, List<Vector3>();

//        Dictionary < Transform, Dictionary<int, int>> vertexIdDict = //vertexDirecionaryForEachBone[bone];

        for (int n = 0; n < targetSkinned.bones.Length; ++n)
        {
            var bone = targetSkinned.bones[n];
            vertexDirecionaryForEachBone[bone] = new Dictionary<int, Dictionary<int, int>>();

            newTriangles[bone] = new List<int>();

            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector4> tangent = new List<Vector4>();
            List<Color> colors = new List<Color>();
            List<Vector2> uv = new List<Vector2>();
            List<Vector2> uv2 = new List<Vector2>();
            List<Vector2> uv3 = new List<Vector2>();
            //List<int> triangles = new List<int>();

            var verticesList = dictForHierachialMeshes[bone];

            //vertexDirecionaryForEachBone[bone] = new Dictionary<int, int>();

            Dictionary<int, int> origNewIndexPair = new Dictionary<int, int>();

            for (int i = 0; i < verticesList.Count-1; ++i)
            {
                Debug.Log(i);
                int origVertIdx = verticesList[i][i*3];
                vertices.Add(origMesh.vertices[origVertIdx]);
                normals.Add(origMesh.normals[origVertIdx]);
                //if (origMesh.colors.Length < i)
                //{
                //newColors.Add(origMesh.colors[origVertIdx]); 
                //}
                tangent.Add(origMesh.tangents[origVertIdx]);
                //if (origMesh.uv.Length < i)
                //{
                uv.Add(origMesh.uv[origVertIdx]);
                //}
                //if (origMesh.uv2.Length < i)
                //{
                //newUV2.Add(origMesh.uv2[origVertIdx]);
                //}
                //if (origMesh.uv3.Length < i)
                //{
                //newUV3.Add(origMesh.uv3[origVertIdx]);
                //}
                vertexDirecionaryForEachBone[bone].Add(i, new Dictionary<int, int>());
                //vertexDirecionaryForEachBone[bone].Add(origVertIdx, i);
                
                origNewIndexPair.Add(origVertIdx, i);
            }
            //for (int i = 0; i < verticesList.Count; ++i)
            //{
            //    int origVertIdx = verticesList[i];
            //    newTriangles[bone].Add( );
            //    newTriangles[bone].Add( );
            //    newTriangles[bone].Add();
            //}
            for (int i = 0; i < verticesList.Count-1; ++i)
            {
                int[] origTriangles = dictForHierachialMeshes[bone][i*3];//new int[3] { origMesh.triangles[verticesList[i]] , 0  , 1};


                newTriangles[bone].Add(origNewIndexPair[ origTriangles[0]]);
                newTriangles[bone].Add(origNewIndexPair[origTriangles[1]]);
                newTriangles[bone].Add(origNewIndexPair[origTriangles[2]]);
                //vertexDirecionaryForEachBone[bone][i].Add(verticesList[i] , origNewIndexPair[i]);
            }
        }

       

        for (int n = 0; n < targetSkinned.bones.Length; ++n)
        {
            var bone = targetSkinned.bones[n];
            newMesh.SetVertices(newVertices[bone]);
            newMesh.SetNormals(newNormals[bone]);
            newMesh.SetTangents(newTangent[bone]);
            //newMesh.SetColors(newColors[bone]);
            newMesh.SetUVs(0, newUV[bone]);
            //newMesh.SetUVs(1, newUV2[bone]);
            //newMesh.SetUVs(2, newUV3[bone]);
            newMesh.SetTriangles(newTriangles[bone], 0);

            if (bone.TryGetComponent(out MeshFilter filter))
            {
                filter.mesh = newMesh;
            }
            else
            {
                bone.AddComponent<MeshFilter>().mesh = newMesh;
            }
            if (bone.TryGetComponent(out MeshRenderer renderer))
            {
                renderer.material = targetSkinned.sharedMaterial;
            }
            else
            {
                bone.AddComponent<MeshRenderer>().material = targetSkinned.sharedMaterial;
            }
        }
        //for (int i = 0; i < newTriangles.Count; ++i)
        //{
        //    newMesh.SetTriangles(newTriangles[i],i);
        //}
        //newMesh.
        //newMesh.SetTriangles(newTriangles , 0);

      
    }
}
