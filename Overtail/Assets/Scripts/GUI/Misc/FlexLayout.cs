using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Overtail.GUI
{
    /// <summary>
    /// IT'S BROKEN, DONT TOUCH
    /// </summary>
    public class FlexLayout : LayoutGroup
    {
        [SerializeField] [Min(1)] public int rows = 1;
        [Min(1)] public int columns = 1;

        public Vector2 spacing;
        public Vector2 size;

        public ConstraintType constraint;


        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();


            if (constraint == ConstraintType.ForceSize)
            {
                AutoLayout();
            }
            else
            {
                AutoSize();
            }


            for (int i = 0; i < rectChildren.Count; i++)
            {
                var r = i / columns;
                var c = i % columns;

                var x = (size.x + spacing.x) * c + padding.left;
                var y = (size.y + spacing.y) * r + padding.top;

                var child = rectChildren[i];

                SetChildAlongAxis(child, 0, x, size.x);
                SetChildAlongAxis(child, 1, y, size.y);
            }
        }

        private void AutoLayout()
        {
            var parentWidth = rectTransform.rect.width;
            var parentHeight = rectTransform.rect.height;

            var c = (parentWidth + spacing.x - padding.left - padding.right) / (size.x + spacing.x);
            columns = (int) c;
            rows = Mathf.CeilToInt((float) transform.childCount / columns);
        }

        private void AutoSize()
        {
            var parentWidth = rectTransform.rect.width;
            var parentHeight = rectTransform.rect.height;

            columns = constraint == ConstraintType.ForceColumns
                ? columns
                : Mathf.CeilToInt(transform.childCount / (float) rows);
            ;

            rows = constraint == ConstraintType.ForceRows
                ? rows
                : Mathf.CeilToInt(transform.childCount / (float) columns);


            var xCutOff = (spacing.x * (columns - 1) + padding.left + padding.right) / columns;

            var yCutOff = (spacing.y * (rows - 1) + padding.top + padding.bottom) / rows;

            size.x = parentWidth / columns - xCutOff;
            size.y = parentHeight / rows - yCutOff;
        }


        public override void CalculateLayoutInputVertical()
        {
        }

        public override void SetLayoutHorizontal()
        {
        }

        public override void SetLayoutVertical()
        {
        }
    }

    public enum ConstraintType
    {
        ForceRows,
        ForceColumns,
        ForceSize
    }
}
