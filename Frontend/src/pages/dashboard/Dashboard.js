import axios from 'axios';
import { useEffect, useState } from 'react'
import { useNavigate } from "react-router-dom";
import DashboardHeader from '../../components/dashboardHeader/DashboardHeader';
import SupportTicketSection from '../../components/ZendeskTickets/SupportTicketSection';
import JiraEpicSection from '../../components/jiraEpics/JiraEpicSection';
import styles from "./Dashboard.module.css"

function Dashboard() {
  const BASE_URL = window.BASE_URL;
  const [loggedIn,setLoggedIn] = useState(false)
  const navigate = useNavigate();
  useEffect(() => {
  checkLogin();
  })
  async function checkLogin() {
  const logintoken = localStorage.getItem('token');
  const userType = localStorage.getItem('userType');
    const config = { headers: { Authorization: `Bearer ${logintoken}` } };
    if (logintoken)
    {
      try {
        const url = BASE_URL + "/Auth/login";
        await axios
          .get(url, config);
        if (userType === "Staff") {
          setLoggedIn(true)
        }
        else {
          navigate('/')
        }
      } 
      catch (error) {
        navigate('/');
      }
    } 
    else 
    {
      navigate('/');
    }
}
  return (
    <>
      {loggedIn && <div id={styles.dashboard}>
        <DashboardHeader/>
        <JiraEpicSection />
        <SupportTicketSection/>
      </div>}
    </>
);
}

export default Dashboard