using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class ConstantCodeGen
{
    private static void ImitateStaticClass(CodeTypeDeclaration type)
    {
        @type.TypeAttributes |= TypeAttributes.Sealed;

        @type.Members.Add(new CodeConstructor {
            Attributes = MemberAttributes.Private | MemberAttributes.Final
        });
    }
    
    private static CodeCompileUnit GenerateClassWithConstants(string name, IReadOnlyList<string> constants, bool isIds = false)
    {
        var compileUnit = new CodeCompileUnit();
        var @namespace = new CodeNamespace("SquareDino.Utils");
        var @class = new CodeTypeDeclaration(name);

        ImitateStaticClass(@class);

        if (isIds)
        {
            for (int i = 0; i < constants.Count; i++)
            {
                if(string.IsNullOrEmpty(constants[i])) continue;
                
                var @const = GenerateConstant(constants[i], i);
                @class.Members.Add(@const);
            }
        }
        else
        {
            foreach (var constantName in constants)
            {
                if(string.IsNullOrEmpty(constantName)) continue;
                
                var @const = GenerateConstant(constantName, constantName);
                @class.Members.Add(@const);
            }
        }

        @namespace.Types.Add(@class);
        compileUnit.Namespaces.Add(@namespace);

        return compileUnit;
    }
    
    private static CodeMemberField GenerateConstant<T>(string name, T value)
    {
        name = name.Replace(" ", "");

        var @const = new CodeMemberField(
            typeof(T),
            name);

        @const.Attributes &= ~MemberAttributes.AccessMask;
        @const.Attributes &= ~MemberAttributes.ScopeMask;
        @const.Attributes |= MemberAttributes.Public;
        @const.Attributes |= MemberAttributes.Const;

        @const.InitExpression = new CodePrimitiveExpression(value);
        return @const;
    }
    
    private static void WriteIntoFile(string fullPath, CodeCompileUnit code)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
        using (var stream = new StreamWriter(fullPath, append: false))
        {
            var writer = new IndentedTextWriter(stream);
            using (var codeProvider = new CSharpCodeProvider())
            {
                codeProvider.GenerateCodeFromCompileUnit(code, writer, new CodeGeneratorOptions());
            }
        }
    }
    
    [MenuItem("Window/SquareDino/Generate Layers Constants")]
    private static void GenerateLayersConstantFile()
    {
        const string path = @"Scripts/Utils/Layers.cs";

        var fullPath = Path.Combine(Application.dataPath, path);
        var className = Path.GetFileNameWithoutExtension(fullPath);

        var code = GenerateClassWithConstants(className, GetAllLayers());
        WriteIntoFile(fullPath, code);

        AssetDatabase.ImportAsset("Assets/" + path, ImportAssetOptions.ForceUpdate);
        AssetDatabase.Refresh();
    }
    
    [MenuItem("Window/SquareDino/Generate LayersIds Constants")]
    private static void GenerateLayersIdsConstantFile()
    {
        const string path = @"Scripts/Utils/LayersIds.cs";

        var fullPath = Path.Combine(Application.dataPath, path);
        var className = Path.GetFileNameWithoutExtension(fullPath);

        var code = GenerateClassWithConstants(className, GetAllLayers(), true);
        WriteIntoFile(fullPath, code);

        AssetDatabase.ImportAsset("Assets/" + path, ImportAssetOptions.ForceUpdate);
        AssetDatabase.Refresh();
    }
    
    [MenuItem("Window/SquareDino/Generate Tag Constants")]
    private static void GenerateTagConstantFile()
    {
        const string path = @"Scripts/Utils/Tags.cs";

        var fullPath = Path.Combine(Application.dataPath, path);
        var className = Path.GetFileNameWithoutExtension(fullPath);

        var code = GenerateClassWithConstants(className, GetAllTags());
        WriteIntoFile(fullPath, code);

        AssetDatabase.ImportAsset("Assets/" + path, ImportAssetOptions.ForceUpdate);
        AssetDatabase.Refresh();
    }
    
    private static List<string> GetAllLayers()
    {
        var layers = new List<string>();
        
        for (var i = 0; i < 32; i++)
        {
            layers.Add(LayerMask.LayerToName(i));   
        }

        return layers;
    }

    private static List<string> GetAllTags()
    {
        return InternalEditorUtility.tags.ToList();
    }
}
