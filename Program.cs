using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Linq;
using static Cat;

// Class to represent the position of an object
public class Position
{
    private double x;
    private double y;
    private double z;

    public double X
    {
        get { return x; }
        set { x = value; }
    }

    public double Y
    {
        get { return y; }
        set { y = value; }
    }

    public double Z
    {
        get { return z; }
        set { z = value; }
    }

    public Position(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }


    public void Move(Animal al, double dx, double dy, double dz)
    {
        double newX = X + dx;
        double newY = Y + dy;
        double newZ = Z + dz;

        if (al is Cat || al is Snake)
        {
            X = Clamp(newX, 0.0, 100.0);
            Y = Clamp(newY, 0.0, 100.0);
            Z = 0.0;
        }
        else
        {
            X = Clamp(newX, 0.0, 100.0);
            Y = Clamp(newY, 0.0, 100.0);
            Z = Clamp(newZ, 0.0, 10.0);
        }

    }


    private double Clamp(double value, double min, double max)
    {
        return Math.Max(Math.Min(value, max), min);
    }


    public override string ToString()
    {
        return $"({X:F1}, {Y:F1}, {Z:F1})";
    }

}
// Enum to represent cat breeds
enum Breed
{
    Abyssinian,
    AmericanWirehair,
    Bengal,
    Himalayan,
    Ocicat,
    Serval
}

// Base class for animals
public abstract class Animal : IComparable<Animal>
{
    private static int count = 0;
    public int ID { get; private set; }
    public string Name { get; set; }
    public double Age { get; set; }
    public Position Pos { get; set; }

    public Animal(int id, string name, double age, Position position)
    {
        ID = id;
        Name = name;
        Age = age;
        Pos = position;
    }


    // Method to move the animal by dx, dy, dz

    //{
    //    Pos.Move(dx, dy, dz);
    //}

    public int CompareTo(Animal other)
    {
        return Name.CompareTo(other.Name);
    }

    public abstract void Eat(Bird bird, ref Bird[] birdArray);

    // Method to convert the animal to string representation
    public override string ToString()
    {
        return $"ID: {ID}, Name: {Name}, Age: {Age}, Position: {Pos}";
    }
}

// Cat class derived from Animal
class Cat : Animal
{
    public Breed Breed { get; set; }


    public Cat(int id, string name, double age, Position position, Breed breed)
        : base(id, name, age, position)
    {
        Breed = breed;
    }



    public override void Eat(Bird bird, ref Bird[] birdArray)
    {
        Console.WriteLine($"Cat {Name} is eating {bird.Name}.");
        birdArray = birdArray.Where(b => b != bird).ToArray();
    }

    public override string ToString()
    {
        return $"{base.ToString()}, Breed: {Breed}";
    }
}


// Subclass for snakes
class Snake : Animal
{
    public double Length { get; set; }
    public bool Venomous { get; set; }


    public Snake(int id, string name, double age, Position position, double length, bool venomous)
       : base(id, name, age, position)
    {
        Length = length;
        Venomous = venomous;
    }
    public override void Eat(Bird bird, ref Bird[] birdArray)
    {
        Console.WriteLine($"Snake {Name} is eating {bird.Name}.");
        birdArray = birdArray.Where(b => b != bird).ToArray();
    }


    public override string ToString()
    {
        return base.ToString() + $", Length: {Length}, Venomous: {Venomous}";
    }
}
// Bird class derived from Animal
public class Bird : Animal
{
    private static int nextID = 0;
    private static List<string> birdNames = new List<string> { "Tweety", "Zazu", "Iago", "Hula", "Manu", "Couscous", "Roo", "Tookie", "Plucky", "Kiwi" };
    public static List<string> BirdNames
    {
        get
        {
            return birdNames;
        }
    }
    public Bird(string name, double age, Position position)
        : base(nextID++, name, age, position)
    {
    }


    public override void Eat(Bird bird, ref Bird[] birdArray)
    {
        // Birds don't eat other birds
    }

    public override string ToString()
    {
        return $"{base.ToString()}";
    }
}



// Custom ArrayList implementation
class ArrayList<T>
    : IEnumerable<T> where T : IComparable<T>
{
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < count; i++)
        {
            yield return array[i];
        }
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    private T[] array;
    private int count;

    public ArrayList()
    {
        array = new T[2];
        count = 0;
    }

    private void Grow()
    {
        Array.Resize(ref array, array.Length * 2);
    }

    public void AddFront(T item)
    {
        if (GetCount() == array.Length)
            Grow();

        Array.Copy(array, 0, array, 1, count);
        array[0] = item;
        count++;
    }

    public void AddLast(T item)
    {
        if (GetCount() == array.Length)
            Grow();

        array[count] = item;
        count++;
    }

    public int GetCount()
    {
        return count;
    }

    public void InsertBefore(T item, T beforeItem)
    {
        int index = Array.IndexOf(array, beforeItem);
        if (index == -1)
            return;

        if (GetCount() == array.Length)
            Grow();

        Array.Copy(array, index, array, index + 1, count - index);
        array[index] = item;
        count++;
    }

    public void InPlaceSort()
    {
        Array.Sort(array);
    }

    public void Swap(int index1, int index2)
    {
        if (index1 < 0 || index1 >= GetCount() || index2 < 0 || index2 >= GetCount())
            return;

        T temp = array[index1];
        array[index1] = array[index2];
        array[index2] = temp;
    }

    public void DeleteFirst()
    {
        if (GetCount() == 0)
            return;

        Array.Copy(array, 1, array, 0, count - 1);
        count--;
    }

    public void DeleteLast()
    {
        if (GetCount() == 0)
            return;

        array[GetCount() - 1] = default(T);
        count--;
    }

    public void RotateLeft()
    {
        if (GetCount() == 0)
            return;

        T temp = array[0];
        Array.Copy(array, 1, array, 0, count - 1);
        array[count - 1] = temp;
    }

    public void RotateRight()
    {
        if (GetCount() == 0)
            return;

        T temp = array[count - 1];
        Array.Copy(array, 0, array, 1, GetCount() - 1);
        array[0] = temp;
    }

    public static ArrayList<T> Merge(ArrayList<T> list1, ArrayList<T> list2)
    {
        ArrayList<T> mergedList = new ArrayList<T>();

        for (int i = 0; i < list1.GetCount(); i++)
            mergedList.AddLast(list1[i]);

        for (int i = 0; i < list2.GetCount(); i++)
            mergedList.AddLast(list2[i]);

        return mergedList;
    }

    //public override string ToString()
    //{
    //    StringBuilder sb = new StringBuilder();
    //    for (int i = 0; i < count; i++)
    //    {
    //        sb.AppendLine(array[i].ToString());
    //    }
    //    return sb.ToString();
    //}
    public string StringPrintAllForward()
    {
        StringBuilder sb = new StringBuilder();

        foreach (T item in this)
        {
            sb.AppendLine(item.ToString());
        }

        return sb.ToString();
    }
    public string StringPrintAllReverse()
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = count - 1; i >= 0; i--)
        {
            stringBuilder.AppendLine(this[i].ToString());
        }
        return stringBuilder.ToString();
    }
    public void DeleteAll(T[] array)
    {
        Array.Clear(array, 0, array.Length);
    }


    public T this[int index]
    {
        get { return array[index]; }
        set { array[index] = value; }
    }
}


class Program
{
    static void Main(string[] args)
    {

        //Console.SetWindowSize(200, 70);
        //Console.SetBufferSize(200, 70);
        Console.CursorVisible = false;
        //The above statements are not working on a Mac

        Random random = new Random();
        // Create an ArrayList of Cats
        ArrayList<Animal> catList = new ArrayList<Animal>();
        for (int i = 0; i < 3; i++)
        {
            Position position = new Position(random.NextDouble() * 30, random.NextDouble() * 30, 0);
            Breed[] catBreedValues = (Breed[])Enum.GetValues(typeof(Breed));
            Breed randomBreed = catBreedValues[random.Next(0, catBreedValues.Length)];

            Cat cat = new Cat(i, $"Cat{i + 1}", random.Next(1, 10), position, randomBreed);
            catList.AddFront(cat);
        }

        // Create an ArrayList of Snakes
        ArrayList<Animal> snakeList = new ArrayList<Animal>();
        for (int i = 0; i < 3; i++)
        {
            Position position = new Position(random.NextDouble() * 30, random.NextDouble() * 30, 0);
            Snake snake = new Snake(i, $"Snake{i + 1}", random.Next(1, 10), position, random.NextDouble() * 10, random.Next(2) == 0);
            snakeList.AddLast(snake);
        }



        // Merge the two lists
        ArrayList<Animal> animalList = ArrayList<Animal>.Merge(catList, snakeList);

        Console.WriteLine("ArrayLists after merging:");
        foreach (Animal animal in animalList)
        {
            Console.WriteLine(animal.ToString());
        }

        // Test PrintAllForward and PrintAllReverse
        Console.WriteLine("PrintAllForward:");
        Console.WriteLine(animalList.StringPrintAllForward());
        Console.WriteLine("PrintAllReverse:");
        Console.WriteLine(animalList.StringPrintAllReverse());

        // Create an array of 10 birds
        Bird[] birdArray = new Bird[10];
        for (int i = 0; i < birdArray.Length; i++)
        {
            string name = Bird.BirdNames[random.Next(Bird.BirdNames.Count)];
            double x = random.NextDouble() * 100;
            double y = random.NextDouble() * 70;
            double z = random.NextDouble() * 10;
            Position position = new Position(x, y, z);
            birdArray[i] = new Bird(name, i, position);
        }


        int roundCount = 0;

        while (birdArray.Length > 0)
        {
            // Print the grid
            Console.Clear();
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.WriteLine("Cats, Snakes, and Birds:");

            for (int y = 0; y <= 70; y++)
            {
                for (int x = 0; x <= 100; x++)
                {
                    if (x == 0 && y == 0 || x == 100 && y == 70)
                    {
                        Console.Write("++");
                    }
                    else if (x == 0 || x == 100)
                    {
                        Console.Write("||");
                    }
                    else if (y == 0 || y == 70)
                    {
                        Console.Write("--");
                    }
                    else
                    {
                        bool catSnakeFound = false;
                        bool catSnakeBirdFound = false;
                        foreach (Animal animal in animalList)
                        {
                            if (
                                Math.Floor(animal.Pos.X) == (double)x &&
                                Math.Floor(animal.Pos.Y) == (double)y
                            )
                            {
                                catSnakeFound = catSnakeBirdFound = true;

                                string letter;
                                if (animal is Cat) letter = "C";
                                else if (animal is Snake) letter = "S";
                                else letter = "?";
                                Console.Write(letter + animal.ID);
                            }
                        }

                        //if (!catSnakeFound)
                        //{
                        foreach (Bird bird in birdArray)
                        {
                            if (
                                Math.Floor(bird.Pos.X) == (double)x &&
                                Math.Floor(bird.Pos.Y) == (double)y
                            )
                            {
                                //Console.SetCursorPosition(x, y);
                                Console.Write("B" + bird.ID);
                                catSnakeBirdFound = true;
                                //Console.SetCursorPosition(0, Console.CursorTop);
                            }
                        }
                        //}
                        if (!catSnakeBirdFound)
                        {
                            Console.Write("  ");
                        }
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Positions:");

            // Print the positions of cats, snakes, and birds
            foreach (Animal animal in animalList)
            {
                Console.WriteLine(animal.Name + animal.ID + " at X: " + animal.Pos.X + ", Y: " + animal.Pos.Y);
            }

            // Cats and snakes eat birds in range
            List<Bird> birdsEaten = new List<Bird>();
            foreach (Animal animal in animalList)
            {
                Bird birdInRange = GetBirdInRange(animal, birdArray);
                if (birdInRange != null)
                {
                    animal.Eat(birdInRange, ref birdArray);
                }
                else
                {
                    Bird nearestBird = GetNearestBird(animal, birdArray);
                    if (nearestBird == null)
                    {
                        break;
                    }
                    MoveTowards(animal, nearestBird);
                }
            }
            //Moving birds randomly
            for (int i = 0; i < birdArray.Length; i++)
            {
                birdArray[i].Pos.Move(birdArray[i], random.NextDouble() * 20.0 - 10.0, random.NextDouble() * 20.0 - 10.0, random.NextDouble() * 4.0 - 2.0);
            }
            roundCount++;

            Console.WriteLine($"Birds left: {birdArray.Length}");

            // Exit if all birds are eaten or after 50 rounds
            if (birdArray.Length == 0)
            {
                break;
            }

            // Delay for visualization
            System.Threading.Thread.Sleep(500);

        } // end of while (birdArray.Length)

        Console.WriteLine();
        Console.WriteLine("Simulation ended.");
        Console.WriteLine("Number of rounds: " + roundCount);
    }

    // Method to get the nearest bird within range of a given animal
    static Bird GetBirdInRange(Animal animal, Bird[] birdArray)
    {
        foreach (Bird bird in birdArray)
        {
            //if (bird.Pos.Z == 0)
            //{
            double distance = Math.Sqrt(Math.Pow(animal.Pos.X - bird.Pos.X, 2) +
                                        Math.Pow(animal.Pos.Y - bird.Pos.Y, 2) +
                                        Math.Pow(animal.Pos.Z - bird.Pos.Z, 2));
            if (distance <= EatingRange(animal))
            {
                return bird;
            }
            //}
        }
        return null;
    }

    // Method to get the nearest bird to a given animal
    static Bird GetNearestBird(Animal animal, Bird[] birdArray)
    {
        if (birdArray.Length == 0)
        {
            return null;
        }



        Bird nearestBird = birdArray[0];

        double minDistance = Math.Sqrt(Math.Pow(animal.Pos.X - nearestBird.Pos.X, 2) +
                                       Math.Pow(animal.Pos.Y - nearestBird.Pos.Y, 2));

        foreach (Bird bird in birdArray)
        {
            double distance = Math.Sqrt(Math.Pow(animal.Pos.X - bird.Pos.X, 2) +
                                        Math.Pow(animal.Pos.Y - bird.Pos.Y, 2));
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestBird = bird;
            }
        }

        return nearestBird;
    }
    public static double EatingRange(Animal animal)
    {
        if (animal is Cat)
            return 8.0;
        else
            return 3.0;
    }
    public static double MovingRange(Animal animal)
    {
        if (animal is Cat)
            return 16.0;
        else
            return 14.0;
    }
    public static void MoveTowards(Animal animal, Bird bird)
    {
        double distX = bird.Pos.X - animal.Pos.X;
        double distY = bird.Pos.Y - animal.Pos.Y;

        double distLen = Math.Sqrt(Math.Pow(distX, 2.0) + Math.Pow(distY, 2.0));
        double distScl = Math.Min(MovingRange(animal), distLen) / distLen;

        double moveX = distX * distScl;
        double moveY = distY * distScl;

        animal.Pos.Move(animal, moveX, moveY, 0);
    }


}