using Roomates.Repositories;
using Roommates.Models;
using Roommates.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=BOOK-DQQQHIR1J7\SQLEXPRESS;database=Roommates;integrated security=true;TrustServerCertificate=true;";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository RoommateRepo = new RoommateRepository(CONNECTION_STRING);

            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Update a room"):
                        List<Room> roomOptions = roomRepo.GetAll();
                        foreach (Room r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to update? ");
                        int selectedRoomId = int.Parse(Console.ReadLine());
                        Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

                        Console.Write("New Name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.Write("New Max Occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

                        roomRepo.Update(selectedRoom);

                        Console.WriteLine("Room has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Delete a room"):
                        List<Room> availableRooms = roomRepo.GetAll();
                        foreach (Room r in availableRooms)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to delete? ");
                        int chosenRoomId = int.Parse(Console.ReadLine());
                        Room chosenRoom = availableRooms.FirstOrDefault(r => r.Id == chosenRoomId);

                        roomRepo.Delete(chosenRoomId);

                        Console.WriteLine("Room has been successfully deleted");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Show all chores"):
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore r in chores)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Show unassigned chores"):
                        List<Chore> UAchores = choreRepo.GetAllUnassigned();
                        foreach (Chore r in UAchores)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Assign chore to a roommate"):
                        Console.WriteLine("Choose a chore to assign:");
                        List<Chore> allChores = choreRepo.GetAll();
                        foreach (Chore r in allChores)
                        {
                            Console.WriteLine($"{r.Id}: {r.Name}");
                        }
                        int chosenChore = int.Parse(Console.ReadLine());

                        Console.WriteLine("Choose a roommate to assign:");
                        List<Roommate> allRoommates = RoommateRepo.GetAll();
                        foreach (Roommate r in allRoommates)
                        {
                            Console.WriteLine($"{r.Id}: {r.FirstName}");
                        }
                        int chosenRoommate = int.Parse(Console.ReadLine());

                        choreRepo.AssignChoreInsert(chosenRoommate, chosenChore);
                        break;

                    case ("Update a chore"):
                        List<Chore> choreOptions = choreRepo.GetAll();
                        foreach (Chore r in choreOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name}");
                        }

                        Console.Write("Which chore would you like to update? ");
                        int selectedChoreId = int.Parse(Console.ReadLine());
                        Chore selectedChore = choreOptions.FirstOrDefault(r => r.Id == selectedChoreId);

                        Console.Write("New Name: ");
                        selectedChore.Name = Console.ReadLine();

                        choreRepo.Update(selectedChore);

                        Console.WriteLine("Chore has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Delete a chore"):
                        List<Chore> availableChores = choreRepo.GetAll();
                        foreach (Chore r in availableChores)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name}");
                        }

                        Console.Write("Which room would you like to delete? ");
                        int deleteChoreId = int.Parse(Console.ReadLine());
                        Chore deleteChore = availableChores.FirstOrDefault(r => r.Id == deleteChoreId);

                        choreRepo.Delete(deleteChoreId);

                        Console.WriteLine("Chore has been successfully deleted");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Add a room",
                "Update a room",
                "Delete a room",
                "Show all chores",
                "Show unassigned chores",
                "Assign chore to a roommate",
                "Update a chore",
                "Delete a chore",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}