namespace VacationMachine.Email;

public interface IEmailSender
{
    void Send(string msg);
}