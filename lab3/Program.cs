
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace lab3
{
	[TestFixture]

	public class Tests
    {
		[Test]
		public void MoreTest()
		{
			Assert.IsTrue(new SemVersion("1.1.0") > new SemVersion("1.0.0"));
		}

		[Test]
		public void LessTest()
		{
			Assert.IsTrue(new SemVersion("1.0.0") < new SemVersion("1.1.0"));
		}

		[Test]
		public void MoreOrEqualTest()
		{
			Assert.IsTrue(new SemVersion("1.1.0") >= new SemVersion("1.0.0"));
			Assert.IsTrue(new SemVersion("1.0.0") >= new SemVersion("1.0.0"));
		}

		[Test]
		public void LessOrEqualTest()
		{
			Assert.IsTrue(new SemVersion("1.0.0") <= new SemVersion("1.1.0"));
			Assert.IsTrue(new SemVersion("1.0.0") <= new SemVersion("1.0.0"));
		}

		[Test]
		public void EqualTest()
		{
			Assert.IsTrue(new SemVersion("1.0.0") == new SemVersion("1.0.0"));
			Assert.IsTrue(new SemVersion("1.1.0") == new SemVersion("1.1.0"));
		}

		[Test]
		public void NoEqualTest()
		{
			Assert.IsTrue(new SemVersion("1.0.0") != new SemVersion("1.1.0"));
			Assert.IsTrue(new SemVersion("1.1.0") != new SemVersion("1.0.0"));
		}

		[Test]
		public void ToStringTest()
		{
			int v1 = 1;
			int v2 = 1;
			int v3 = 0;
			string a;
			string b;

			string a1 = v1.ToString();
			string a2 = v2.ToString();
			string a3 = v3.ToString();


			a = (a1 + "." + a2 + "." + a3);
			b = Convert.ToString("1.1.0");

			Assert.IsTrue(a == b);
			Assert.IsFalse(a != b);
		}

		[Test]
        public void IntersectionTest() // Пересечение
        {
			Assert.AreEqual(new VersionsInterval(new SemVersion("0.7.7"), new SemVersion("1.5.2")).ToString(), VersionsInterval.Intersection(new VersionsInterval(">0.7.6"), new VersionsInterval("<=1.5.2"))[0].ToString());
			Assert.AreEqual(new VersionsInterval(new SemVersion("4.0.0"), new SemVersion("4.5.7")).ToString(), VersionsInterval.Intersection(new VersionsInterval(">3.1.1 <=4.5.7"), new VersionsInterval(">=4.0.0 <8.0.1"))[0].ToString());
			Assert.AreEqual(new VersionsInterval(new SemVersion("3.1.4"), new SemVersion("5.0.9")).ToString(), VersionsInterval.Intersection(new VersionsInterval(">0.0.0 <=9.3.1"), new VersionsInterval(new SemVersion("3.1.4"), new SemVersion("5.0.9")))[0].ToString());
			Assert.AreEqual((new VersionsInterval(new SemVersion("4.0.0"), new SemVersion("5.0.0")).ToString()), VersionsInterval.Intersection(new VersionsInterval(">=1.0.0 <=5.0.0"), new VersionsInterval(new SemVersion("4.0.0"), new SemVersion("7.0.0")))[0].ToString());

			Assert.AreEqual((new VersionsInterval(new SemVersion("3.0.0"), new SemVersion("5.0.0")).ToString()), VersionsInterval.Intersection(new VersionsInterval(">=2.0.0 <=5.0.0"), new VersionsInterval(">=3.0.0 <=7.0.0"))[0].ToString());

			Assert.AreEqual(VersionsInterval.Intersection(new VersionsInterval("<2.0.0"), new VersionsInterval(">3.0.0")), null);

			Assert.AreEqual(VersionsInterval.Intersection(new VersionsInterval(">3.0.0"), new VersionsInterval("<2.0.0")), null);
		}
		[Test]
        public void UnionTest() //Объединение 
        {
			Assert.AreEqual(new VersionsInterval(new SemVersion("0.0.0"), new SemVersion("2.4.1")).ToString(), VersionsInterval.Union(new VersionsInterval(">0.7.6 <=2.4.1"), new VersionsInterval("<=1.5.2")).ToString());
			Assert.AreEqual(new VersionsInterval(new SemVersion("3.1.2"), new SemVersion("8.0.0")).ToString(), VersionsInterval.Union(new VersionsInterval(">3.1.1 <=4.5.7"), new VersionsInterval(">=4.0.0 <8.0.1")).ToString());
			Assert.AreEqual(new VersionsInterval(new SemVersion("0.0.1"), new SemVersion("9.3.1")).ToString(), VersionsInterval.Union(new VersionsInterval(">0.0.0 <=9.3.1"), new VersionsInterval(new SemVersion("3.1.4"), new SemVersion("5.0.9"))).ToString());

			Assert.AreEqual(VersionsInterval.Nonunion(new VersionsInterval("<=2.0.0"), new VersionsInterval(">=3.0.0 <=4.0.0")), "0.0.0 2.0.0 || 3.0.0 4.0.0");
			Assert.AreEqual(VersionsInterval.Nonunion(new VersionsInterval("<=2.0.0"), new VersionsInterval(">=3.0.0")), "0.0.0 2.0.0 || 3.0.0 2147483647.2147483647.2147483647");
			Assert.AreEqual(VersionsInterval.Union(new VersionsInterval(new SemVersion("0.0.0"), new SemVersion("2.0.0")), new VersionsInterval(new SemVersion("1.0.0"), new SemVersion("3.0.0"))).ToString(), new VersionsInterval(new SemVersion("0.0.0"), new SemVersion("3.0.0")).ToString());

		}

		[Test]
		public void Equality()
		{
			Assert.IsTrue(new VersionsInterval(new SemVersion("1.6.1"), new SemVersion("3.1.3")) == new VersionsInterval(new SemVersion("1.6.1"), new SemVersion("3.1.3")));
			Assert.IsTrue(new VersionsInterval(new SemVersion("1.0.0"), new SemVersion("3.0.0")) != new VersionsInterval(new SemVersion("1.6.1"), new SemVersion("3.1.3")));
			Assert.IsFalse(new VersionsInterval(new SemVersion("1.6.0"), new SemVersion("3.1.3")) == new VersionsInterval(new SemVersion("1.6.1"), new SemVersion("3.1.3")));
			Assert.IsFalse(new VersionsInterval(new SemVersion("1.0.0"), new SemVersion("3.0.0")) != new VersionsInterval(new SemVersion("1.0.0"), new SemVersion("3.0.0")));
		}
	}
}
