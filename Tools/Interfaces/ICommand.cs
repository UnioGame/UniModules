using System.Collections;

public interface ICommand
{
    void Execute();
    bool Rollback();
}

public interface ICommandRoutine
{
    IEnumerator Execute();   
}
