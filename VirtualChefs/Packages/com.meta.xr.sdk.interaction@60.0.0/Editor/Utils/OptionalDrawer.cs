/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using UnityEngine;
using UnityEditor;

namespace Oculus.Interaction.Editor
{
    /// <summary>
    /// Adds an (Optional) label in the inspector next to any SerializedField with this attribute.
    /// </summary>
    [CustomPropertyDrawer(typeof(OptionalAttribute))]
    public class OptionalDrawer : DecoratorDrawer
    {
        private const string _label = "(Optional)";
        private const string _labelAuto = "(Generated if missing)";
        private const float _indentWidth = 15f; //from the internal constant EditorGUI.kIndentPerLevel

        public override void OnGUI(Rect position)
        {
            OptionalAttribute optionalAtt = attribute as OptionalAttribute;
            bool isAuto = (optionalAtt.Flags & OptionalAttribute.Flag.AutoGenerated) != 0;
            string label = isAuto ? _labelAuto : _label;
            GUIStyle style = new GUIStyle(EditorStyles.miniLabel);
            style.normal.textColor = new Color(style.normal.textColor.r, style.normal.textColor.g, style.normal.textColor.b, 0.55f);

            Vector2 size = style.CalcSize(new GUIContent(label));

            position.height = EditorGUIUtility.singleLineHeight;
            position.x += EditorGUIUtility.labelWidth - size.x - (EditorGUI.indentLevel * _indentWidth);

            EditorGUI.LabelField(position, label, style);
        }

        public override float GetHeight()
        {
            return 0f;
        }
    }
}
