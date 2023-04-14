using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Utils;

public class ToolsManager : MonoBehaviour
{
    #region PublicProperty
    [Header("Display")]
    public RawImage inputImgComponent;
    public RawImage outputImgComponent;

    [Header("File")]
    public Cubemap m_cubemap;

    [Header("Shaders")]
    public ComputeShader cubemapToSpheircalmap;

    [Header("Configure")]
    public bool isWriteFile = false;
    #endregion

    void Start()
    {
        RenderTexture m_spherical = CubemapToSpheircal(cubemapToSpheircalmap, m_cubemap, 1024, 512);
        outputImgComponent.texture = m_spherical;

        if (isWriteFile)    QFile.SaveRenderTexture(m_spherical, "SphericalMap2.png");
    }


    public RenderTexture CubemapToSpheircal(ComputeShader cm2smShader, Cubemap cubeMapTexture, int width, int height)
    {
        int kernelHandle = cm2smShader.FindKernel("Cm2SmMain");

        cm2smShader.GetKernelThreadGroupSizes(kernelHandle, out uint x, out uint y, out uint z);
        Vector3Int dispatchCounts = new Vector3Int(Mathf.CeilToInt((float)1024 / (float)x),
                                                   Mathf.CeilToInt((float)768 / (float)y),
                                                   Mathf.CeilToInt((float)6 / (float)z));

        RenderTexture cm2smResult = new RenderTexture(1024, 512, 6);
        cm2smResult.enableRandomWrite = true;
        cm2smResult.Create();

        cm2smShader.SetTexture(kernelHandle, "Result", cm2smResult);
        cm2smShader.SetTexture(kernelHandle, "_CubeMapTexture", cubeMapTexture);
        cm2smShader.SetVector("_DispatchCount", (Vector3)dispatchCounts);
        cm2smShader.SetVector("_TextureSize", new Vector3(1024, 768, 6));
        cm2smShader.Dispatch(kernelHandle, dispatchCounts.x, dispatchCounts.y, dispatchCounts.z);

        return cm2smResult;
    }
}
