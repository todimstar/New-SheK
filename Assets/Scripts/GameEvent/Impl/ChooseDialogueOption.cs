/// <summary>
/// 对话选项选项事件
/// </summary>
public struct ChooseDialogueOption:IEvent
{
    public void OnEvent(){}

    public int optionIndex;

    public ChooseDialogueOption(int optionIndex)
    {
        this.optionIndex = optionIndex;
    } 
}