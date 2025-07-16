namespace task01;
public static class StringExtensions{
        public static bool IsPalindrome(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            var filtered = new System.Text.StringBuilder();
            foreach (char c in input)
            {
                if (!char.IsWhiteSpace(c) && !char.IsPunctuation(c))
                {
                    filtered.Append(char.ToLower(c));
                }
            }
            
            if (filtered.Length == 0)
                return false;

            int left = 0;
            int right = filtered.Length - 1;
            
            while (left < right)
            {
                if (filtered[left] != filtered[right])
                    return false;
                
                left++;
                right--;
            }
            
            return true;
        }
}
