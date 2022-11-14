import axios from 'axios';
import { useEffect, useState } from 'react'
import { useNavigate } from "react-router-dom";
import DashboardHeader from '../../components/dashboardHeader/DashboardHeader';
import SupportTicketSection from '../../components/SupportTicketSection';
import JiraEpicSection from '../../components/jiraEpics/JiraEpicSection';
import styles from "./Dashboard.module.css"

function Dashboard() {
  const [loggedIn,setLoggedIn] = useState(false)
  const navigate = useNavigate();
  useEffect(() => {
  checkLogin();
  })
  async function checkLogin() {
  const logintoken = localStorage.getItem('token')
    const config = { headers: { Authorization: `Bearer ${logintoken}` } };
    if (logintoken)
    {try {
      await axios
        .get("https://localhost:7001/Auth/login", config);
      setLoggedIn(true)
    } catch (error) {
      navigate('/');
      }
    } else {
      navigate("/");
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