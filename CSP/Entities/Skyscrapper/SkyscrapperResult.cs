using System;
using System.Text;
using CSP.Entities.Futoshiki;

namespace CSP.Entities.Skyscrapper
{
    public class SkyscrapperResult
    {
        public SkyscrapperVariable[,] Board { get; }
        public SkyscrapperConstraints Constraints { get; }
        public string Title { get; }
        public int NodesVisitedCount { get; }
        public TimeSpan ElapsedTime { get; }

        public SkyscrapperResult(string title, SkyscrapperVariable[,] board, int nodesVisitedCount, TimeSpan elapsedTime, SkyscrapperConstraints constraints)
        {
            Board = board;
            NodesVisitedCount = nodesVisitedCount;
            ElapsedTime = elapsedTime;
            Constraints = constraints;
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
            sb.Append("<table>");
            sb.Append("<tr><td></td>");
            for (int i = 0; i < Constraints.TopEdge.Count; i++)
            {
                int top = Constraints.TopEdge[i].HasValue ? Constraints.TopEdge[i].Value : 0;
                sb.Append($"<td style=\"border-left: none; font-size: 20px; padding: 7px;\"><b>{top}</b></td>");
            }
            sb.Append("<td></td></tr>");
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                sb.Append("<tr style=\"border: 1px solid black;\">");
                int left = Constraints.LeftEdge[i].HasValue ? Constraints.LeftEdge[i].Value : 0;
                sb.Append($"<td style=\"border-left: none; font-size: 20px; padding: 7px;\"><b>{left}</b></td>");
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    var value = Board[i, j].Value != null ? Board[i, j].Value : 0;
                    sb.Append($"<td style=\"border: 1px solid black; font-size: 20px; padding: 7px;\">{value}</td>");
                }
                int right = Constraints.RightEdge[i].HasValue ? Constraints.RightEdge[i].Value : 0;
                sb.Append($"<td style=\"border-right: none; font-size: 20px; padding: 7px;\"><b>{right}</b></td>");
                sb.Append("</tr>");
            }
            sb.Append("<tr><td></td>");
            for (int i = 0; i < Constraints.BottomEdge.Count; i++)
            {
                int bottom = Constraints.BottomEdge[i].HasValue ? Constraints.BottomEdge[i].Value : 0;
                sb.Append($"<td style=\"border-left: none; font-size: 20px; padding: 7px;\"><b>{bottom}</b></td>");
            }
            sb.Append("<td></td></tr>");
            sb.Append("</table>");
            sb.Append($"<h4>Nodes visited: {NodesVisitedCount}</h4>");
            sb.Append($"<h4>Elapsed time: {ElapsedTime}</h4>");
            return sb.ToString();
        }
    }
}