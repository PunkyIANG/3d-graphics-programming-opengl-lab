namespace OpenTKTest.Source.Utils;

public static class Prefabs
{
    public static Model GetQuad() => ModelLoader.LoadBasicModel("Models/Quad/quad.obj");
    public static Model GetTripleArrow() => ModelLoader.LoadTexturedModel("Models/ThreeArrows/threearrows.obj", "Models/ThreeArrows/threearrows.mtl");
    public static Model GetArrow() => ModelLoader.LoadBasicModel("Models/Arrow/arrow.obj");
    public static Model GetTexturedQuad() => ModelLoader.LoadTexturedModel("Models/Quad/quad.obj","Models/Quad/quad.mtl");

}