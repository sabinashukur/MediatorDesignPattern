namespace MediatorDesignPattern;

//ChatMediator interface contains the AddUser and SendMessageToAllUsers methods and ChatMediator implements this interface.
//All users inherits from the User abstract class.
public interface IChatMediator
{
    void AddUser(User user);
    void SendMessageToAllUsers(string message, User currentUsr);
}

public class ChatMediator : IChatMediator
{
    private List<User> _users;

    public ChatMediator()
    {
        _users = new List<User>();
    }

    public void AddUser(User user)
    {
        _users.Add(user);
    }

    public void SendMessageToAllUsers(string message, User currentUsr)
    {
        _users.ForEach(w =>
        {
            if (w != currentUsr)    //don't send message to sender
            {
                w.ReceiveMessage(message);
            }
        });
    }
}
//User class have chatMediator and name variables and two methods name SendMessage and ReceiveMessage.

public abstract class User
{
    private IChatMediator chatMediator;
    private string name;

    public string Name
    {
        get
        {
            return name;
        }
    }

    public IChatMediator ChatMediator
    {
        get
        {
            return chatMediator;
        }
    }
    public abstract void SendMessage(string message);
    public abstract void ReceiveMessage(string message);
    public User(IChatMediator chatMediator, string name)
    {
        this.chatMediator = chatMediator;
        this.name = name;
    }
}

//BasicUser have a reference to ChatMediator class and in the send message method,
//it calls the SendMessageToAllUsers method to send it's message to all users.
//BasicUser doesn't know about the others BasicUsers and PremiumUsers.
public class BasicUser : User
{
    public BasicUser(IChatMediator chatMediator, string name)
        : base(chatMediator, name)
    {
    }

    public override void SendMessage(string message)
    {
        this.ChatMediator.SendMessageToAllUsers(message, this);
    }

    public override void ReceiveMessage(string message)
    {
        Console.WriteLine("Basic User: " + this.Name + " receive message: " + message);
    }
}

public class PremiumUser : User
{
    public PremiumUser(IChatMediator chatMediator, string name)
        : base(chatMediator, name)
    {
    }

    public override void SendMessage(string message)
    {
        this.ChatMediator.SendMessageToAllUsers(message, this);
    }

    public override void ReceiveMessage(string message)
    {
        Console.WriteLine("Premium User: " + this.Name + " receive message: " + message);
    }
}

class Program
{
    static void Main(string[] args)
    {
        IChatMediator chatMediator = new ChatMediator();

        //3 users are online;
        User user1 = new BasicUser(chatMediator, "Fariz");
        User user2 = new PremiumUser(chatMediator, "Sevinc");
        User user3 = new PremiumUser(chatMediator, "Aysu");
        chatMediator.AddUser(user1);
        chatMediator.AddUser(user2);
        chatMediator.AddUser(user3);

        //Sabir added to the chat room
        User newUser = new BasicUser(chatMediator, "Sabir");
        chatMediator.AddUser(newUser);

        newUser.SendMessage("Sabir is online.");
        //Basic User: Fariz receive message: Sabir is online.
        //Premium User: Sevinc receive message: Sabir is online.
        //Premium User: Aysu receive message: Sabir is online.
    }
}

