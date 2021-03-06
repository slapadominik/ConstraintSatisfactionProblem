﻿using System;
using System.Text;

namespace CSP.Entities.Futoshiki
{
    public class FutoshikiResult
    {
        public FutoshikiVariable[,] Board { get; }
        public string Title { get; }
        public int NodesVisitedCount { get; }
        public TimeSpan ElapsedTime { get; }

        public FutoshikiResult(string title, FutoshikiVariable[,] board, int nodesVisitedCount, TimeSpan elapsedTime)
        {
            Board = board;
            NodesVisitedCount = nodesVisitedCount;
            ElapsedTime = elapsedTime;
            Title = title;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Board \n");
            if (Board != null)
            {
                for (int i = 0; i < Board.GetLength(0); i++)
                {
                    for (int j = 0; j < Board.GetLength(1); j++)
                    {
                        sb.Append(Board[i, j]);
                        sb.Append(" ");
                    }
                    sb.AppendLine();
                }
            }
            sb.AppendLine($"Nodes visited: {NodesVisitedCount}");
            sb.AppendLine($"Elapsed time [milliseconds]:{ElapsedTime.Milliseconds}");
            return sb.ToString();
        }

        public string ToHtml()
        {
            StringBuilder sb = new StringBuilder($"<h3>File: {Title}</h3>");
            sb.Append("<table style=\"border: 1px solid black; border-collapse: collapse;\">");
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                sb.Append("<tr style=\"border: 1px solid black;\">");
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    var value = Board[i, j].Value != null ? Board[i, j].Value : 0;
                    sb.Append($"<td style=\"border: 1px solid black; font-size: 20px; padding: 7px;\">{value}</td>");
                }

                sb.Append("</tr>");
            }
            sb.Append("</table>");
            sb.Append($"<h4>Nodes visited: {NodesVisitedCount}</h4>");
            sb.Append($"<h4>Elapsed time: {ElapsedTime}</h4>");
            return sb.ToString();
        }
    }
}