# SuperScan
Supernova scanning automation using TheSkyX Pro
(This description can be found in Word Document form in the source files:  SuperScan Description.docx)

SuperScan Automation Application
(Beta)
Overview
SuperScan is a Windows-compatible application whose purpose is to automate the search for supernova suspects using the TheSkyX™ Professional astro-imaging platform.  
Each night that SuperScan is run, a fresh set of images is taken of all available galaxies that fit a set of user-stipulated criteria.  Each galactic image is compared against the most recent equivalent image from a prior SuperScan run.  If any new light sources appear within the boundaries of the imaged galaxy, their location is derived and flagged.  Upon completion of a scan, the images that contain flagged suspects are displayed for visual analysis and any follow up work a user might want to prove the source real.
As options, SuperScan supports variable exposure length, autofocus, delayed start, execution of pre-scan and post-scan automation scripts, minimum altitude limit, and filter selection.  Additional constraints on the galaxy selection can be made by modifications to the SuperScan database search query.
  
Feature Description
For each night’s session, SuperScan uses TheSkyX™ to generate a list of imageable target galaxies in the form of a constrained Observing List.  For each target galaxy, SuperScan takes an image using a single filter, normally clear.  Unless this is the first image of that galaxy, this image is compared against the most recent image taken of that target.  If an unexpected light source is detected, then the target is flagged for the user’s subsequent attention as a suspect.  The newest image is then banked for comparison during the next scan.  SuperScan images each available target until the list is exhausted.
Once the light source processing on a scan is completed, the list of suspects is made available for visual analysis.  Using TSX, the prior image, current image and difference image are displayed.  The light source anomaly is centered on the TSX chart.  The user can then mark the suspect as cleared or leave for follow up imaging and analysis.  Suspect information is retained in an XML file.
If the user wishes to optimize the use of available imaging time during a scan, the detection function of SuperScan can be suspended.  Detection of light sources can take up to several minutes for each target depending upon computer capabilities.  At a later time, the detection function can be executed.  In this mode, no images are acquired, but the two most recent images for every galaxy in the image bank are compared and analyzed.   
The scan does not use auto-guiding, but relies on the inherent tracking precision of mounts such as the Software Bisque Paramount™ family.  Once such a mount is fully dialed-in, images of up to five minutes exhibit light source trailing artifacts that are indistinguishable from the noise contributions of seeing and bloom.  This attribute may limit the minimum magnitude of viable targets, but significantly reduces the complexity of targeting, as well as improves its speed and reliability.
SuperScan attempts to minimize slew distances and meridian flips.  After a galaxy is imaged, the next galaxy selected will be the closest unimaged galaxy that is on the same side of the meridian as the last, and is still above the minimum altitude.  When the last galaxy on one side of the meridian is imaged, the scan will continue with the northern-most image on the opposite side of the meridian
By way of configuration, the user may choose exposure time, minimum target altitude and imaging filter.  The user can also modify the galaxy search query to control the characteristics of the galaxies chosen for scanning.  
Another feature is the capability to run start the galaxy scan at a user-defined time.  When using “AutoStart”, the user sets the start time and selects optional, user-built, automation script files for running before and after the galaxy scan.  These scripts can be used for opening and closing a dome, for instance.
A user can also choose to have TSX refocus the camera (using @Focus2) upon initialization and whenever the focus temperature changes by at least one degree.
Information on how to assess and report new astronomical discoveries can be found at:
https://www.iau.org/public/themes/discoveries/

Configuration
	•	Exposure Time: Sets the length of exposure in seconds for each image
	•	Minimum Altitude: Restricts galaxy targets to this lower limit.  Important over long scans.
	•	Filter Number: Sets the filter to be used for all images during the scan.  Zero-based.
	•	Postpone Detection: Images will not be tested for anomalies during the scan.  Saves some time.
	•	AutoFocus: The camera will be focused at the start and whenever the temperature changes by one degree while the scan is underway.
	•	AutoStart: The scan will start at a time designated in a pop up window.  Optionally a pre-scan and post-scan application can be configured to run using the same window.
	•	Always On Top: The SuperScan application window will always show on top of all other windows.
	Command Buttons
	•	Scan and Detect: Starts the scan.  Each image will be tested for anomalous light sources after imaging, unless Postpone Detection is selected.
	•	Detect:  Detection will be run on the most recent two images in the image Bank galaxy folders.  This command is normally run after a scan with Postpone Detection set, say, in the morning.
	•	Examine:  A popup window is generated wherein the user can select logged suspects for further review.  Once selected, TSX will display the cropped reference, current and difference images and configure the sky chart to show the location of the anomaly.
	•	Close:  Ends the SuperScan session.  Close also serves to abort the scan session after completion of the current image capture, if desired.
 Requirements
SuperScan is a Windows Forms executable, written in Visual C#.  The app requires TheSkyX Professional (Build 10837 or later) with the TSX Camera Add-On option. The application runs as an uncertified, standalone application under Windows 7, 8 and 10.  
SuperScan has been validated on a Paramount™ MX+.  The developer assumes that other precision mounts from such vendors as AstroPhysics can also support accurate imaging over longer exposures without autoguiding, but makes no guarantees.
Installation
Download the SuperScan_Exe.zip and open. Run the "Setup" application.  Upon completion, an application icon will have been added to the start menu under "TSXToolKit" with the name "SuperScan".  This application can be pinned to the Start if desired.
Support
This application was written for the public domain and as such is unsupported. The developer would happily entertain questions or suggestions, and may update the application occasionally as time permits.  Otherwise, the developer wishes you his best and hopes everything works out, but recommends learning Visual C# (it's not hard and the tools are free from Microsoft) if you find a problem or want to add features.  The source is supplied as a Visual Studio project.
 
Detailed Description
Configuration and Initialization
Data File Structure
SuperScan files are stored in the user’s Documents folder in a folder named “SuperScan”.  In this folder will be stored:
	•	A directory named “Logs” containing daily log files::
	o	Text-based log files with dated names:  “dd_mmm_yyyy.txt”
	•	A directory named “Image Bank” containing galaxy image directories:
	o	Directories with the name “NGC xxxx” where xxxx is the NGC number.: Within each directory are the galaxy images.
		Uncropped images with naming format:
	 “NGC xxxx_yyyy-MM-dd HHmm.fit”
	•	Cropped image of most recent image: “CurrentImage.fit”
	•	Cropped image of next most recent image “Reference.fit”
	•	Cropped image of difference image: “Difference.fit”

	•	An XML-based configuration file named “SuperScanCfg.xml”
	•	An XML-based suspect file named “suspects.xml”
Focus: 
When selected, @Focus2 is run at the beginning of a scan and whenever the temperature (measured at the focuser) changes by one degree.  TSX should be configured to choose and slew to the focus star.
Rotation:
SuperScan adjusts for differences between image PA when comparing sequential images
Filter:
The user can configure the filter to be used for the SuperScan session, based on a zero-based filter number in the filter wheel.  Normally, this would be the Clear or Luminance filter as it provides for the fastest imaging, but the user may choose some other filter for some other reason
Exposure:
Exposure time can be set by the user, and normally should be set between 3 and 5 minutes.  The automatic light source detection routine will compensate for some variation in brightness between subsequent images of the same, but not too much variation will cause false positives or negatives..  So, the exposure time should be set and left for all sessions.  This may be improved upon in the future.  Maybe not.
AutoStart
A checkbox determines if SuperScan will delay until a proscribed time to begin scanning.  If AutoStart is selected, a configuration window will pop up for the user to enter the location of an executable file to run just before the galaxy scan, the location of an executable file to run just after the galaxy scan, and the time to start the scan.  If the time entered is earlier than the current time, then the start time will be set for the next day, e.g. after midnight of the current day.
Galaxy List Generation
The galaxy list is driven by a custom TheSkyX™ Observing List query “SuperScanQuery.dbs”.  This query searches the NGC catalog for all galaxies, within the user’s local horizon whose:
•	Major axis size exceeds 5 arcseconds.
•	Magnitude is greater than 14.   
•	Altitude is greater than 30°.
These query parameters will produce on the order of 10 to 150 target galaxies, that require up to four hours of scan time at an exposure time of 60 seconds.  The query parameters can be manually changed by the user if desired by using the TSX Manage Observing List function.  Future versions of SuperScan may make this query more easily configurable.  Maybe not.
Fresh Image Collection
For each galaxy in the list, SuperScan performs a Closed Loop Slew to the galaxy location as defined by the TSX Find function.  Then,
	Calibration is set for AutoDark – no options. 
	Filter is unchanged. 
	Exposure time – see Configuration. 
 
The image is taken asynchronously although SuperScan has nothing else to do while waiting, for now..  Aborts are not supported at this time.  Upon completion, the image is stored as a temporary file in anticipation of comparison with a previous image.
Galaxy Image Bank
The Image Bank is a set of folders where previously taken images of each galaxy are sorted and stored.  Each folder is named with the NGC number of the galaxy, e.g. “NGC 2331” to allow easy lookup.  See Configuration.  Once an imaged is processed, it is stored in the Image Bank.
New Light Source Detection
SuperScan expects a supernova explosion to appear as a new, stellar-size light source within a galactic image.  “New” in this case means that TSX astrometry (Image Link) detects a light source the current image that is not detected in the most recent prior image.  “Within the galactic image” means that the light source is within the area of the galaxy as defined by the major and minor axis.  
For each galaxy imaged during a session, SuperScan adjusts the two images to a minimum overall pixel difference:
1.	Extract TSX WCS Inventory information to determine relative rotation and translation 
2.	Crop images based on galaxy major axis.  Convert cropped images to image arrays and apply bias, rotation and translation adjustments.
3.	Compare average pixel values (TSX) to determine relative image exposure levels.
4.	Adjust and subtract cropped reference image from current image.  Save the cropped reference, current and difference images as fits files in the galaxy’s Image Bank folder.
5.	Remaining light sources are extracted from the difference image.  Their location is compared with cropped reference image light sources.  If a difference image light sources has no reference image light sources near it, then it is flagged, x/y location on the difference image recorded and RA/Dec location from the current image computed.
6.	The session moves on to the next galaxy.
Suspect Review
Upon completion of an Evaluation (anomalous light source detection), information about the suspect light source is stored in an XML file:  Suspects.xml.  Each entry includes the galaxy name, image time and position of the suspect light source.  Upon selection, SuperScan uses TSX to display the prior image, current image and difference image.  The TSX chart is centered on the suspect position and FOV is set to the size of the associated galaxy, such that the images can be visually analyzed.  Upon completion, the status of the suspect can be set to “Cleared” or left “Suspected” for later analysis.
Logging
SuperScan maintains an xml-based session log in the base folder under the filenames called “SuperScan<date>.xml”.  Progress is also logged to a text box in the main window.


