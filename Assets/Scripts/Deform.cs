using UnityEngine;

public class Deform : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] vertices;
    
    // Defina a largura das bases do trapézio e a altura
    [SerializeField] private float baseWidthTop = 1.0f;
    [SerializeField] private float baseWidthBottom = 2.0f;
    [SerializeField] private float height = 1.0f;
    [SerializeField] private float depth = 1.0f;

    void Start()
    {
        // Obtém o Mesh do cubo e armazena seus vértices originais
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        vertices = (Vector3[])originalVertices.Clone(); // Clone para manipulação
    }
    
    void Update()
    {
        // Verifica se a tecla espaço foi pressionada
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DeformToTrapezoid();
        }
    }
    
    void DeformToTrapezoid()
    {
        // Calcule os offsets para a deformação
        float halfBaseWidthTop = baseWidthTop / 2.0f;
        float halfBaseWidthBottom = baseWidthBottom / 2.0f;
        float halfDepth = depth / 2.0f;

        // Definir os novos vértices do cubo em formato de trapézio
        vertices[0] = new Vector3(-halfBaseWidthBottom, -0.5f * height, -halfDepth); // Base inferior esquerda
        vertices[1] = new Vector3(halfBaseWidthBottom, -0.5f * height, -halfDepth);  // Base inferior direita
        vertices[2] = new Vector3(halfBaseWidthTop, 0.5f * height, -halfDepth);       // Base superior direita
        vertices[3] = new Vector3(-halfBaseWidthTop, 0.5f * height, -halfDepth);      // Base superior esquerda
        vertices[4] = new Vector3(-halfBaseWidthBottom, -0.5f * height, halfDepth);  // Base inferior esquerda traseira
        vertices[5] = new Vector3(halfBaseWidthBottom, -0.5f * height, halfDepth);   // Base inferior direita traseira
        vertices[6] = new Vector3(halfBaseWidthTop, 0.5f * height, halfDepth);        // Base superior direita traseira
        vertices[7] = new Vector3(-halfBaseWidthTop, 0.5f * height, halfDepth);       // Base superior esquerda traseira

        // Atualiza o Mesh com os novos vértices
        mesh.vertices = vertices;
        mesh.RecalculateNormals(); // Recalcula as normais para uma visualização correta
    }
}
