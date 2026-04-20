using Loopie;
using System;
using System.Text;

class EnemyList : Component
{
    class EnemyDirection
    {
        public string EnemyName;
        public Entity ReferenceEntity;
    }

    void OnCreate()
    {

    }

    void OnUpdate()
    {

    }

    string FormatName(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        const string suffix = "_Reference";

        if (input.EndsWith(suffix))
            input = input.Substring(0, input.Length - suffix.Length);

        StringBuilder result = new StringBuilder();

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            if (i > 0
                && char.IsUpper(c)
                && !char.IsUpper(input[i - 1]))
            {
                result.Append(' ');
            }

            result.Append(c);
        }
        return result.ToString();
    }
};