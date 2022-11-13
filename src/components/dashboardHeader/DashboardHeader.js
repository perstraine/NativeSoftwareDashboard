import Logo from "./assets/Logo.png";
import styles from "./DashboardHeader.module.css";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
export default function DashboardHeader() {
  useEffect(() => {
    getInfo();
  }, []);
    const navigate = useNavigate();
  const [active, setActive] = useState(0);
  const [urgent, setUrgent] = useState(0);
  const [closed, setClosed] = useState(0);
  async function getInfo() {
    try {
      const response = await axios.get(
        "https://localhost:7001/api/DashboardInfo"
      );
      setActive(response.data.activeTickets);
      setUrgent(response.data.urgentTickets);
      setClosed(response.data.closedTickets);
    } catch (error) {}
    }
    function logout() {
      localStorage.removeItem("token");
      navigate("/");
    }
  return (
    <div id={styles.headerContainer}>
      <div id={styles.logoContainer}>
        <img src={Logo} id={styles.logo} alt="NativeSoftware Logo" />
      </div>
      <div className={styles.infoContainer}>
        <div className={styles.infoTitle}>Active Support Tickets</div>
        <div className={styles.infoValue}>{active}</div>
      </div>
      <div className={styles.infoContainer}>
        <div className={styles.infoTitle}>Urgent Support Tickets</div>
        <div className={styles.infoValue}>{urgent}</div>
      </div>
      <div className={styles.infoContainer}>
        <div className={styles.infoTitle}>Closed Tickets This Week</div>
        <div className={styles.infoValue}>{closed}</div>
          </div>
          <div id={styles.logout} onClick={logout}>Logout</div>
    </div>
  );
}
