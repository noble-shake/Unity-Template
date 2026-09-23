namespace RottenNoble.Contents.Feature.Request
{
    /// <summary>
    /// Command의 입력. readonly struct로 둔다 — 실행 중에 호출부가 값을 바꿀 수 없게 하기 위해서다.
    /// View나 ViewModel을 필드로 담지 않는다. 담는 순간 Command가 화면 수명에 묶인다.
    /// </summary>
    public readonly struct FeatureOpenRequest
    {
        public string Address { get; }

        public FeatureOpenRequest(string address)
        {
            Address = address;
        }
    }
}
