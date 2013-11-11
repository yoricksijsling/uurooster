using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;

namespace OsirisClient
{
	public class Schedule
	{
		public readonly DateTime CreatedAt = DateTime.Now;
		public readonly List<Month> Months = new List<Month>();

		public DateTime CreatedAtCET {
			get { return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(CreatedAt, "CET"); }
		}

		public static Schedule Parse(Stream input)
		{
			var schedule = new Schedule();

			var doc = new HtmlDocument();
			doc.Load(input);

			Month currentMonth = null;
			Day currentDay = null;
			foreach (HtmlNode rowNode in doc.DocumentNode.SelectNodes("//table[@class=\"OraTableContent\"][1]/tr")) {
				HtmlNode monthNode = rowNode.SelectSingleNode("td[1]/span");
				if (monthNode != null) {
					schedule.Months.Add(currentMonth = new Month(monthNode.InnerText));
					continue;
				}

				HtmlNode timeNode = rowNode.SelectSingleNode("td[1]/table/tr");
				if (timeNode != null) { // This row contains rooster data
					var dayOfWeek = timeNode.SelectSingleNode("td[1]").InnerText.Trim();
					var dayOfMonth = timeNode.SelectSingleNode("td[2]").InnerText.Trim();
					if (dayOfWeek != "" && dayOfMonth != "") {
						currentMonth.Add(currentDay = new Day(dayOfMonth, dayOfWeek));
					}

					var time = timeNode.SelectSingleNode("td[3]").InnerText.Replace(" ", "");

					var fieldNodes = rowNode.SelectNodes("td[position() mod 2 = 0]");
					var fields = fieldNodes.Select(node => node.InnerText.Trim()).ToList();

					var highlight = fieldNodes[2].SelectSingleNode("span[@class='psbToonTekstRood']") != null;

					currentDay.Add(new Row(
						currentMonth.Name, currentDay.DayOfWeek, currentDay.DayOfMonth, time,
						fields[0], fields[1], fields[2], fields[3], fields[4], fields[5], fields[6], highlight));
				}
			}

			return schedule;
		}

		public class Month : List<Day>
		{
			public string Name;

			public Month(string name)
			{
				Name = name;
			}
		}

		public class Day : List<Row>
		{
			public string DayOfMonth;
			public string DayOfWeek;

			public Day(string dayOfMonth, string dayOfWeek)
			{
				DayOfMonth = dayOfMonth;
				DayOfWeek = dayOfWeek;
			}
		}
		
		public class Row
		{
			public readonly string Month;
			public readonly string DayOfWeek;
			public readonly string DayOfMonth;
			public readonly string Time;
			public readonly string CourseCode;
			public readonly string Course;
			public readonly string Kind;
			public readonly string Group;
			public readonly string Building;
			public readonly string Room;
			public readonly string Other;
			public readonly bool Highlight;

			public Row(string month, string dayOfWeek, string dayOfMonth, string time, string courseCode, string course, string kind, string group, string building, string room, string other, bool highlight)
			{
				Month = month;
				DayOfWeek = dayOfWeek;
				DayOfMonth = dayOfMonth;
				Time = time;
				CourseCode = courseCode;
				Course = course;
				Kind = kind;
				Group = group;
				Building = building;
				Room = room;
				Other = other;
				Highlight = highlight;
			}
			

			public override string ToString()
			{
				return string.Format("[Row: Month={0}, DayOfWeek={1}, DayOfMonth={2}, Time={3}, Code={4}, Name={5}, Kind={6}, Group={7}, Building={8}, Room={9}, Other={10}, Highlight={11}]",
				                     Month, DayOfWeek, DayOfMonth, Time, CourseCode, Course, Kind, Group, Building, Room, Other, Highlight);
			}
		}
	}
}

