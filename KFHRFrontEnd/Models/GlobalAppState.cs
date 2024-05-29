public class GlobalAppState
{
    public string AuthToken { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public bool IsAdmin { get; set; } = false;

    // Method to set all authentication data at once
    public void SetAuthData(string token, string username, bool isAdmin)
    {
        AuthToken = token;
        Username = username;
        IsAdmin = isAdmin;
    }

    // Additional method to include the user ID
    public void SetAuthData(string token, string username, string userId, bool isAdmin)
    {
        AuthToken = token;
        Username = username;
        UserId = userId;
        IsAdmin = isAdmin;
    }
}