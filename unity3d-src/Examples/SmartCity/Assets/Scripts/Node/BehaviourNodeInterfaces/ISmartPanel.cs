public interface ISmartPanel {

    void RenderDisplay(byte[] image);
    void ClearDisplay();
    void ReceiveInfo(object info);

}
