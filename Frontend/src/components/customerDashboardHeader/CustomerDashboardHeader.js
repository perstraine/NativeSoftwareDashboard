import Logo from "./assets/Logo.png";
import CogWheel from "./assets/cogwheel.png";
import axios from "axios";
import { useEffect, useState, useRef } from "react";
import { useNavigate } from "react-router-dom";
import styles from "./CustomerDashboardHeader.module.css";
import AddZendeskTicket from "../popupWindows/AddZendeskTicket";
import AddJiraRequest from "../popupWindows/AddJiraRequest";
import ViewResponseTime from "../popupWindows/ViewResponseTime";

export default function CustomerDashboardHeader() {
  const BASE_URL = window.BASE_URL;
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

  let customerEmail = localStorage.getItem('email');
  async function getInfo() {
    try {
      let userToken = localStorage.getItem("token");
      const config = { headers: { Authorization: `Bearer ${userToken}`},  params: { email: customerEmail }};
      const url = BASE_URL + "/api/DashboardInfo/Customer";

      const response = await axios.get(url, config);

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

  return (
    <div id={styles.headerContainer} >
      <AddJiraRequest trigger={addJiraRequest} setTrigger={setAddJiraRequest}></AddJiraRequest>
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
        
        <div id={styles.menuContainer}>
          <div id={styles.menuTrigger} onClick={()=>{setOpen(!open)}}>
            <img src={CogWheel} id={styles.cogwheelDropdown} alt="CogWheel" />
          </div>
          {open?
            <div id={styles.dropdownMenu} onClick={() => { setOpen(!open) }}>
              <ul>
              <li className={styles.dropdownItem} onClick={()=>setAddJiraRequest(true)}><h3>New Jira Request</h3></li>
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