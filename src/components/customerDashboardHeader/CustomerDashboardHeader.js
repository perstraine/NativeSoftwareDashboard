import Logo from "./assets/Logo.png";
import styles from "./CustomerDashboardHeader.module.css";
import { useEffect, useState, useRef } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import CogWheel from "./assets/cogwheel.png";
import AddZendeskTicket from "../popupWindows/AddZendeskTicket";
import AddJiraRequest from "../popupWindows/AddJiraRequest";
import AddJiraComment from "../popupWindows/AddJiraComment";
import ViewResponseTime from "../popupWindows/ViewResponseTime";

export default function CustomerDashboardHeader() {
  useEffect(() => {
    getInfo();
  }, []);

  const[addZenTicketPopup, setAddZenTicketPopup] = useState(false);
  const[addJiraRequest, setAddJiraRequest] = useState(false);
  const[addJiraComment, setAddJiraComment] = useState(false);
  const[viewResponseTime, setViewResponseTime] = useState(false);

  const navigate = useNavigate();
  const [active, setActive] = useState(0);
  const [closed, setClosed] = useState(0);
  const [customer, setCustomer] = useState("");
  const [open, setOpen] = useState(false);
  
  let menuRef = useRef();
  useEffect(()=>{
    let handler = (e)=>{
      if(!menuRef.current.contains(e.target)){
        setOpen(false);
      }
    };
    document.addEventListener("mousedown", handler);
  });

  let customerEmail = localStorage.getItem('email');
  async function getInfo() {
    try {
      const response = await axios.get(
        "https://localhost:7001/api/DashboardInfo/Customer",{ params: { email: customerEmail } });
      console.log(response);
      setActive(response.data.activeTickets);
      setClosed(response.data.closedTickets);
      setCustomer(response.data.customer);
    } catch (error) {}
    }
    function logout() {
      localStorage.removeItem("token");
      localStorage.removeItem('userType');
      navigate("/");
    }

  function OpenPopupWindow(){
    setAddZenTicketPopup(true)
  }

  return (
    <div id={styles.headerContainer}>
      <AddJiraRequest trigger={addJiraRequest} setTrigger={setAddJiraRequest}></AddJiraRequest>
      <AddJiraComment trigger={addJiraComment} setTrigger={setAddJiraComment}></AddJiraComment>
      <AddZendeskTicket trigger={addZenTicketPopup} setTrigger={setAddZenTicketPopup}></AddZendeskTicket>
      <ViewResponseTime trigger={viewResponseTime} setTrigger={setViewResponseTime}></ViewResponseTime>
      <div id={styles.logoContainer}>
        <img src={Logo} id={styles.logo} alt="NativeSoftware Logo" />
      </div>
      <div className={styles.infoContainer}>
        <div className={styles.infoTitle}>Active Support Tickets</div>
        <div className={styles.infoValue}>{active}</div>
      </div>
      <div className={styles.infoContainer}>
        <div className={styles.infoTitle}>Closed Tickets This Week</div>
        <div className={styles.infoValue}>{closed}</div>
      </div>
      <div id={styles.settings}>
        <p id={styles.userName}>{customer}</p>
        <div id={styles.logout} onClick={logout}>Logout</div>
        
        <div id={styles.menuContainer} ref={menuRef}>
          <div id={styles.menuTrigger} onClick={()=>{setOpen(!open)}}>
            <img src={CogWheel} id={styles.cogwheelDropdown} alt="CogWheel" />
          </div>
          {open?
            <div id = {styles.dropdownMenu}>
              <ul>
              <li className={styles.dropdownItem} onClick={()=>setAddJiraRequest(true)}><h3>New Jira Request</h3></li>
              <li className={styles.dropdownItem} onClick={()=>setAddJiraComment(true)}><h3>Add Jira Comment</h3></li>
              <li className={styles.dropdownItem} onClick={()=>setAddZenTicketPopup(true)}><h3>Add Zendesk Ticket</h3></li>
              <li className={styles.dropdownItem} onClick={()=>setViewResponseTime(true)}><h3>View Response Time</h3></li>
              </ul>
            </div>
            :
            null
          }
        </div>
      </div>
    </div>
  );
}