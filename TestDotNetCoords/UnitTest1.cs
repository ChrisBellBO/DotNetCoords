using DotNetCoords;
using DotNetCoords.Datum;

namespace TestDotNetCoords
{
  /// <summary>
  /// Summary description for UnitTest1
  /// </summary>
  [TestClass]
  public class UnitTest1
  {
    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get { return testContextInstance; }
      set { testContextInstance = value; }
    }

    #region Additional test attributes

    //
    // You can use the following additional attributes as you write your tests:
    //
    // Use ClassInitialize to run code before running the first test in the class
    // [ClassInitialize()]
    // public static void MyClassInitialize(TestContext testContext) { }
    //
    // Use ClassCleanup to run code after all tests in a class have run
    // [ClassCleanup()]
    // public static void MyClassCleanup() { }
    //
    // Use TestInitialize to run code before running each test 
    // [TestInitialize()]
    // public void MyTestInitialize() { }
    //
    // Use TestCleanup to run code after each test has run
    // [TestCleanup()]
    // public void MyTestCleanup() { }
    //

    #endregion

    [TestMethod]
    public void TestOsRefConstructor()
    {
      var osRef = new OSRef("NJ942063");
      Assert.AreEqual(394200, osRef.Easting);
      Assert.AreEqual(806300, osRef.Northing);

      // test 8 digit grid refs
      var osRef8 = new OSRef("NJ94210631");
      Assert.AreEqual(394210, osRef8.Easting);
      Assert.AreEqual(806310, osRef8.Northing);

      // test 10 digit grid refs
      var osRef10 = new OSRef("NJ9421106311");
      Assert.AreEqual(394211, osRef10.Easting);
      Assert.AreEqual(806311, osRef10.Northing);
    }

    [TestMethod]
    public void TestOsRefConstructorFromLatLng()
    {
      // create OSGB36 LatLng
      var latLng = new LatLng(55.00006367, 3.00141096, 0, OSGB36Datum.Instance);
      var osRef = new OSRef(latLng);
      var newLatLng = osRef.ToLatLng();

      Assert.AreEqual(latLng.Latitude, newLatLng.Latitude, 0.0000001);
      Assert.AreEqual(latLng.Longitude, newLatLng.Longitude, 0.0000001);
      Assert.AreEqual(latLng.Datum, newLatLng.Datum);
    }

    [TestMethod]
    public void TestIrishRefConstructorFromLatLng()
    {
      // create Irish LatLng
      var latLng = new LatLng(54.6027229456799, -5.92249533852385);
      var osRef = new IrishRef(latLng);

      // from https://gnss.osi.ie/new-converter/
      // https://irish.gridreferencefinder.com/ seems to be wrong
      Assert.AreEqual(334301, (int)osRef.Easting);
      Assert.AreEqual(374705, (int)osRef.Northing);

      var newLatLng = osRef.ToLatLng();
      newLatLng.ToWGS84();

      Assert.AreEqual(latLng.Latitude, newLatLng.Latitude, 0.0000001);
      Assert.AreEqual(latLng.Longitude, newLatLng.Longitude, 0.0000001);
      Assert.AreEqual(latLng.Datum, newLatLng.Datum);
    }

    [TestMethod]
    public void TestLatLngConstructor()
    {
      // try lat/lng somewhere in Oz
      LatLng latLng = new LatLng(-26.123, 137.123);
      Assert.AreEqual(-26, latLng.LatitudeDegrees);
      Assert.AreEqual(7, latLng.LatitudeMinutes);
      Assert.AreEqual(22.8, latLng.LatitudeSeconds, 0.01);

      Assert.AreEqual(137, latLng.LongitudeDegrees);
      Assert.AreEqual(7, latLng.LongitudeMinutes);
      Assert.AreEqual(22.7994, latLng.LongitudeSeconds, 0.01);

      Assert.AreEqual(-26.123, latLng.Latitude);
      Assert.AreEqual(137.123, latLng.Longitude);
    }

    [TestMethod]
    public void TestLatLngConstructor2()
    {
      // try lat/lng somewhere in Oz
      LatLng latLng = new LatLng(26, 7, 22.8, NorthSouth.South, 137, 7, 22.7994, EastWest.East);
      Assert.AreEqual(-26, latLng.LatitudeDegrees);
      Assert.AreEqual(7, latLng.LatitudeMinutes);
      Assert.AreEqual(22.8, latLng.LatitudeSeconds, 0.01);

      Assert.AreEqual(137, latLng.LongitudeDegrees);
      Assert.AreEqual(7, latLng.LongitudeMinutes);
      Assert.AreEqual(22.7994, latLng.LongitudeSeconds, 0.01);

      Assert.AreEqual(-26.123, latLng.Latitude, 0.01);
      Assert.AreEqual(137.123, latLng.Longitude, 0.01);
    }

    [TestMethod]
    public void TestLatLngConstructor3()
    {
      var latLng = new LatLng(45, 0, 0, NorthSouth.North, 90, 0, 0, EastWest.West);
      Assert.AreEqual(45, latLng.Latitude);
      Assert.AreEqual(-90, latLng.Longitude);

      latLng = new LatLng(45, 0, 0, NorthSouth.North, 90, 0, 0, EastWest.West, 1);
      Assert.AreEqual(45, latLng.Latitude);
      Assert.AreEqual(-90, latLng.Longitude);

      latLng = new LatLng(45, 0, 0, NorthSouth.North, 90, 0, 0, EastWest.West, 1, WGS84Datum.Instance);
      Assert.AreEqual(45, latLng.Latitude);
      Assert.AreEqual(-90, latLng.Longitude);
    }

    [TestMethod]
    public void TestLatLngHeight()
    {
      LatLng latLng = new LatLng(-26.123, 137.123, 1);
      Assert.AreEqual(1, latLng.Height);
    }

    [TestMethod]
    public void TestUTMRefToString()
    {
      UTMRef utmRef = new UTMRef(1, 'F', 123, 456);
      Assert.AreEqual("1F 123 456", utmRef.ToString());
    }

    [TestMethod]
    public void TestMgrsPrecision()
    {
      var mgrsRef = new MGRSRef("30UVB0257801724");
      var tenMeterPrecision = mgrsRef.ToString(Precision.Precision10M);
      Assert.AreEqual("30UVB02570172", tenMeterPrecision);

      mgrsRef = new MGRSRef("30UVB02570172");
      tenMeterPrecision = mgrsRef.ToString(Precision.Precision10M);
      Assert.AreEqual("30UVB02570172", tenMeterPrecision);
    }

    [TestMethod]
    public void TestMgrsRefConstructor()
    {
      MGRSRef mgrsRef = new MGRSRef("30UPC0486311980");
      Assert.AreEqual(30, mgrsRef.UtmZoneNumber);
      Assert.AreEqual('U', mgrsRef.UtmZoneChar);
      Assert.AreEqual('P', mgrsRef.EastingId);
      Assert.AreEqual('C', mgrsRef.NorthingId);
      Assert.AreEqual(4863, mgrsRef.Easting);
      Assert.AreEqual(11980, mgrsRef.Northing);
      Assert.AreEqual(Precision.Precision1M, mgrsRef.Precision);

      mgrsRef = new MGRSRef(30, 'U', 'P', 'C', 4863, 11980, Precision.Precision1M);
      Assert.AreEqual("30UPC0486311980", mgrsRef.ToString());
    }

    [TestMethod]
    public void TestIsValidLatLng()
    {
      Assert.AreEqual(false, LatLng.IsValidLatitude(-91));
      Assert.AreEqual(true, LatLng.IsValidLatitude(-90));
      Assert.AreEqual(true, LatLng.IsValidLatitude(0));
      Assert.AreEqual(false, LatLng.IsValidLatitude(91));
      Assert.AreEqual(true, LatLng.IsValidLatitude(90));

      Assert.AreEqual(false, LatLng.IsValidLongitude(-181));
      Assert.AreEqual(true, LatLng.IsValidLongitude(-180));
      Assert.AreEqual(true, LatLng.IsValidLongitude(0));
      Assert.AreEqual(false, LatLng.IsValidLongitude(181));
      Assert.AreEqual(true, LatLng.IsValidLongitude(180));
    }

    [TestMethod]
    public void TestED50()
    {
      // convert LatLng using WGS84 to UTM Ref using ED50
      LatLng start = new LatLng(50.7, 4.59);

      start.ToDatum(ED50Datum.Instance);
      Assert.AreEqual(50.700836, start.Latitude, 0.000001);
      Assert.AreEqual(4.591284, start.Longitude, 0.000001);

      MGRSRef mgrs = start.ToMGRSRef();
      Assert.AreEqual("31UFS1237917880", mgrs.ToString());
      Assert.IsInstanceOfType(mgrs.Datum, typeof(ED50Datum));

      //start.ToDatum(ED50Datum.Instance);
      //Assert.AreEqual(55.00088028, start.Latitude, 0.00001);
      //Assert.AreEqual(2.99927935, start.Longitude, 0.00001);
    }

    [TestMethod]
    public void TestCopyAndReverseConversion()
    {
      LatLng start = new LatLng(53.753453, -2.464233);
      LatLng copy = new LatLng(start);

      copy.ToOSGB36();
      copy.ToWGS84();

      Assert.AreEqual(start.Latitude, copy.Latitude, 0.0000001);
      Assert.AreEqual(start.Longitude, copy.Longitude, 0.0000001);

      copy = new LatLng(start);
      copy.ToDatum(OSGB36Datum.Instance);
      copy.ToDatum(WGS84Datum.Instance);

      Assert.AreEqual(start.Latitude, copy.Latitude, 0.0000001);
      Assert.AreEqual(start.Longitude, copy.Longitude, 0.0000001);
    }

    [TestMethod]
    public void CompareToOSGB36WithToDatum()
    {
      LatLng start = new LatLng(55.00006367, 3.00141096);
      start.ToOSGB36();
      Assert.IsInstanceOfType(start.Datum, typeof(OSGB36Datum));

      LatLng usingToDatum = new LatLng(55.00006367, 3.00141096);
      usingToDatum.ToDatum(OSGB36Datum.Instance);
      Assert.IsInstanceOfType(usingToDatum.Datum, typeof(OSGB36Datum));

      Assert.AreEqual(start.Latitude, usingToDatum.Latitude, 0.0000001);
      Assert.AreEqual(start.Longitude, usingToDatum.Longitude, 0.0000001);
    }

    [TestMethod]
    public void CompareToWGS84WithToDatum()
    {
      LatLng start = new LatLng(55, 3, 0, OSGB36Datum.Instance);
      start.ToWGS84();
      Assert.IsInstanceOfType(start.Datum, typeof(WGS84Datum));

      LatLng usingToDatum = new LatLng(55, 3, 0, OSGB36Datum.Instance);
      usingToDatum.ToDatum(WGS84Datum.Instance);
      Assert.IsInstanceOfType(usingToDatum.Datum, typeof(WGS84Datum));

      Assert.AreEqual(start.Latitude, usingToDatum.Latitude, 0.0000001);
      Assert.AreEqual(start.Longitude, usingToDatum.Longitude, 0.0000001);
    }

    [TestMethod]
    public void TestOSRefToGridRef()
    {
      OSRef osRef = new OSRef(518866, 169116);

      // 6 figure grid ref
      string gridRef = osRef.ToSixFigureString();
      Assert.AreEqual("TQ188691", gridRef);

      // 10 figure grid ref
      string gridRef10 = osRef.ToTenFigureString();
      Assert.AreEqual("TQ 18866 69116", gridRef10);
    }

    [TestMethod]
    public void TestOSRefToGridRefWithLeadingZeros()
    {
      OSRef osRef = new OSRef(200010, 10);

      // 6 figure grid ref
      string gridRef = osRef.ToSixFigureString();
      Assert.AreEqual("SX000000", gridRef);

      // 10 figure grid ref
      string gridRef10 = osRef.ToTenFigureString();
      Assert.AreEqual("SX 00010 00010", gridRef10);
    }

    [TestMethod]
    public void TestRoundTrip()
    {
      OSRef osRef = new OSRef(100000, 100000);
      LatLng latLng = osRef.ToLatLng();

      Assert.AreEqual(50.72145, latLng.Latitude, 0.00001);
      Assert.AreEqual(-6.25114, latLng.Longitude, 0.00001);

      latLng.ToWGS84();

      // values from https://www.geocachingtoolbox.com/index.php?lang=en&page=coordinateConversion&status=result
      Assert.AreEqual(50.72197, latLng.Latitude, 0.00001);
      Assert.AreEqual(-6.25202, latLng.Longitude, 0.00001);

      latLng.ToOSGB36();

      OSRef osRef2 = latLng.ToOSRef();

      Assert.AreEqual(osRef.Easting, osRef2.Easting, 0.1);
      Assert.AreEqual(osRef2.Northing, osRef2.Northing, 0.1);
    }

    [TestMethod]
    public void TestECEFRef()
    {
      // tests don't do much at the moment
      var ecefRef = new ECEFRef(1, 1, 1);

      Assert.AreEqual(1, ecefRef.X);
      Assert.AreEqual(1, ecefRef.Y);
      Assert.AreEqual(1, ecefRef.Z);

      var latLng = new LatLng(20, 20);
      ecefRef = new ECEFRef(latLng);
      var newLatLng = ecefRef.ToLatLng();
      Assert.AreEqual(20, newLatLng.Latitude);
      Assert.AreEqual(20, newLatLng.Longitude);

      ecefRef = new ECEFRef(1, 1, 1, WGS84Datum.Instance);
      Assert.AreEqual(1, ecefRef.X);
      Assert.AreEqual(1, ecefRef.Y);
      Assert.AreEqual(1, ecefRef.Z);
    }

    [TestMethod]
    public void TestDatum()
    {
      Assert.AreEqual("World Geodetic System 1984 (WGS84)", WGS84Datum.Instance.Name);

      var wsgLL = new LatLng(51.399423, -0.29749);
      wsgLL.ToDatum(OSGB36Datum.Instance);

      Assert.AreEqual(51.39890, wsgLL.Latitude, 0.00001);
      Assert.AreEqual(-0.29591, wsgLL.Longitude, 0.00001);

      wsgLL.ToDatum(WGS84Datum.Instance);

      Assert.AreEqual(51.399423, wsgLL.Latitude, 0.00001);
      Assert.AreEqual(-0.29749, wsgLL.Longitude, 0.00001);
    }

    [TestMethod]
    public void TestIrishRef()
    {
      var irishRef = new IrishRef(10000, 10000);
      Assert.AreEqual(10000, irishRef.Easting);
      Assert.AreEqual(10000, irishRef.Northing);

      irishRef = new IrishRef("T514131");
      var outStr = irishRef.ToSixFigureString();
      Assert.AreEqual("T514131", outStr);
    }

    [TestMethod]
    public void TestIrishRefToLatLng()
    {
      var irishRef = new IrishRef(128249, 133492);
      var ll = irishRef.ToLatLng();
      ll.ToWGS84();

      // TODO - these seem a bit off
      Assert.AreEqual(52.448612, ll.Latitude, 0.00001);
      Assert.AreEqual(-9.056034, ll.Longitude, 0.00001);
    }

    [TestMethod]
    public void TestLatLngConversions()
    {
      var latLng = new LatLng(20, 10);
      Assert.IsNotNull(latLng.ToDMSString());

      var utmRef = latLng.ToUtmRef();
      var newLatLng = utmRef.ToLatLng();
      Assert.AreEqual(latLng.Latitude, newLatLng.Latitude, 0.0000001);
      Assert.AreEqual(latLng.Longitude, newLatLng.Longitude, 0.0000001);
    }

    [TestMethod]
    public void TestLatLngDistance()
    {
      var latLng = new LatLng(20, 10);
      var latLng2 = new LatLng(20, 10);

      Assert.AreEqual(0, latLng2.Distance(latLng));
      Assert.AreEqual(0, latLng2.DistanceMiles(latLng));

      var latLng3 = new LatLng(53.8360349332714, -2.21262515231851);
      var latLng4 = new LatLng(53.8360349332714, -2.21262515231851);

      Assert.AreEqual(0, latLng3.Distance(latLng4));
      Assert.AreEqual(0, latLng3.DistanceMiles(latLng4));

      var ll5 = new LatLng(54.990221, -1.571044);
      var ll6 = new LatLng(54.584796, -1.219482);

      Assert.IsTrue(ll5.Distance(ll6) > 50);
      Assert.IsTrue(ll5.Distance(ll6) < 51);

      var ll7 = new LatLng(53.836034, -2.212625);
      var ll8 = new LatLng(53.836035, -2.212625);

      Assert.IsTrue(ll7.Distance(ll8) < 0.001);

      var llWestDrayton = new LatLng(51.510054, -0.472234);
      var llIver = new LatLng(51.508502, -0.506726);

      var dWDI = llWestDrayton.Distance(llIver);
      var dI = llIver.Distance(llWestDrayton);
      Assert.AreEqual(dWDI, dI);

      var llKingston = new LatLng(51.41275, -0.301166);
      var llHamptonWick = new LatLng(51.414523, -0.31249);
      var distance = llKingston.Distance(llHamptonWick);
      Assert.IsTrue(distance < 1);
    }

    [TestMethod]
    public void TestMgrsRefConversions()
    {
      MGRSRef mgrsRef = new MGRSRef("25XEN0415886521");
      var str = mgrsRef.ToString();
      Assert.AreEqual("25XEN0415886521", str);

      str = mgrsRef.ToString(Precision.Precision1M);
      Assert.AreEqual("25XEN0415886521", str);
      Assert.AreEqual(false, mgrsRef.IsBessel());

      var utm = mgrsRef.ToUTMRef();
      Assert.IsNotNull(utm);
      Assert.AreEqual("25X 504158 9286521", utm.ToString());
    }

    [TestMethod]
    public void TestMgrsRefRoundTrip()
    {
      var mgrsRef = new MGRSRef("25XEN0415886521");

      var ll = mgrsRef.ToLatLng();

      var rt = ll.ToMGRSRef();
      Assert.AreEqual("25XEN0415886521", rt.ToString());

      mgrsRef = new MGRSRef("12QVJ2053442848");
      ll = mgrsRef.ToLatLng();
      Assert.AreEqual(21.18531, ll.Latitude, 4);
      Assert.AreEqual(-111.76555, ll.Longitude, 4);

      rt = ll.ToMGRSRef();
      Assert.AreEqual("12QVJ2053442848", rt.ToString());
    }

    [TestMethod]
    public void TestLatLngToUtmRef()
    {
      var latlng = new LatLng(44.51601173544275, 12.012860160820502);
      var utmRef = latlng.ToUtmRef();

      Assert.AreEqual("33T 262592 4933528", utmRef.ToString());

      var forcedUtmRef = latlng.ToUtmRef(32);
      Assert.AreEqual("32T 739452 4933603", forcedUtmRef.ToString());
    }

    [TestMethod]
    public void TestBearing()
    {
      var kansas = new LatLng(39.099912, -94.581213);
      var stLouis = new LatLng(38.627089, -90.200203);

      var bearing = kansas.Bearing(stLouis);

      Assert.AreEqual(96.51, bearing, 0.01);
    }

    [TestMethod]
    public void TestItmRefRoundTrip()
    {
      var itmRef = new ITMRef(715830, 734697);
      var irishRef = itmRef.ToIrishRef();

      // test Irish Ref (comparing to https://gnss.osi.ie/new-converter/)
      Assert.AreEqual(234670, (int)irishRef.Northing);
      Assert.AreEqual(315904, (int)irishRef.Easting);

      var newItemRef = irishRef.ToITMRef();

      Assert.AreEqual(itmRef.Northing, (int)Math.Round(newItemRef.Northing));
      Assert.AreEqual(itmRef.Easting, (int)Math.Round(newItemRef.Easting));
    }

    [TestMethod]
    public void TestItmRefToLatLng()
    {
      var itmRef = new ITMRef(715830, 734697);
      var ll = itmRef.ToLatLng();
      ll.ToWGS84();

      // testing lat/lng against conversions on https://irish.gridreferencefinder.com/ and https://gnss.osi.ie/new-converter/
      Assert.AreEqual(ll.Latitude, 53.34979391184205, 0.00001);
      Assert.AreEqual(ll.Longitude, -6.260247772634605, 0.00001);
    }

    [TestMethod]
    public void TestOSRefConstructorVsLatLngToOSRef()
    {
      var ll = new LatLng(51.399444, -0.296911);
      var osRef = ll.ToOSRef();

      var os2 = new OSRef(ll);

      Assert.AreEqual(518572, (int)os2.Easting);
      Assert.AreEqual(168088, (int)os2.Northing);

      Assert.AreEqual(osRef.Easting, os2.Easting);
      Assert.AreEqual(osRef.Northing, os2.Northing);
      Assert.AreEqual(osRef.Datum.Name, os2.Datum.Name);
    }
  }
}
