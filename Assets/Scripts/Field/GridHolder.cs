using UnityEngine;

namespace Scripts.Field
{
    public class GridHolder : MonoBehaviour
    {
        [SerializeField] private int m_GridWight;
        [SerializeField] private int m_GridHeight;

        [SerializeField] private Vector2Int m_Target;
        [SerializeField] private Vector2Int m_Srart;

        [SerializeField] private float m_NodeSize;

        private Grid m_Grid;

        private Camera m_Camera;

        private Vector3 m_Offset;

        private void Start()
        {
            m_Camera = Camera.main;
            
            float wight = m_GridWight * m_NodeSize;
            float height = m_GridHeight * m_NodeSize ;
            
            // Default plane size 10 by 10
            transform.localScale = new Vector3(
                wight * 0.1f,
                1f, 
                height * 0.1f);

            m_Offset = transform.position - (new Vector3(wight, 0f, height) * 0.5f);
            m_Grid = new Grid(m_GridWight, m_GridHeight, m_Offset, m_NodeSize, m_Target);
        }

        private void Update()
        {
            if (m_Grid == null || m_Camera == null)
            {
                return;
            }

            Vector3 mousePosition = Input.mousePosition;

            Ray ray = m_Camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform != transform)
                {
                    return;
                }

                Vector3 hitPosition = hit.point;
                Vector3 difference = hitPosition - m_Offset;

                int x = (int)(difference.x / m_NodeSize);
                int z = (int)(difference.z / m_NodeSize);
                
                Debug.Log(x + " " + z);
            }
        }

        private void OnDrawGizmos()
        {
            if (m_Grid == null)
            {
                return;
            }
            
            Gizmos.color = Color.red;
            
            foreach (Node node in m_Grid.EnumerateAllNodes())
            {
                if (node.NextNode == null)
                {
                    continue;
                }
                Vector3 start = node.Position;
                Vector3 end = node.NextNode.Position;

                Vector3 direction = end - start;

                start -= direction * 0.25f;
                end -= direction * 0.75f;
                
                Gizmos.DrawLine(start, end);
                Gizmos.DrawSphere(end , 0.1f);
            }
        }
    }
}