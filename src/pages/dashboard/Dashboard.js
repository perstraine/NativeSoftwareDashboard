import axios from 'axios';
import { useEffect, useState } from 'react'
import { useNavigate } from "react-router-dom";
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
  function logout() {
    localStorage.removeItem('token')
    navigate('/')
  }
  return (
    <>{loggedIn && <div id={styles.dashboard}>
      <h1>Dashboard</h1>
      <JiraEpicSection />
      {/* <SupportTicketSection/> */}
      <button onClick={logout}>Log Out</button>
    </div>}
      </>

);
}

export default Dashboard