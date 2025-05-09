public class QuestionTextUpdate:IEvent{
    /// <summary>
    /// 任务描述文本
    /// </summary>
    public string text;
    public QuestionTextUpdate(string text){
        this.text=text;
    }
    public void OnEvent(){
    }
}