import Logo from "./assets/Logo.png";
import CogWheel from "./assets/cogwheel.png";
import styles from "./CustomerDashboardHeader.module.css";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import Dropdown from 'react-dropdown';
import 'react-dropdown/style.css';

export default function CustomerDashboardHeader() {
  useEffect(() => {
    getInfo();
  }, []);
  const navigate = useNavigate();
  const [active, setActive] = useState(0);
  const [urgent, setUrgent] = useState(0);
  const [closed, setClosed] = useState(0);
  // const options = ['one', 'two', 'three'];
  // const defaultOption = options[0];
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
      localStorage.removeItem('userType');
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
        <div className={styles.infoTitle}>Closed Tickets This Week</div>
        <div className={styles.infoValue}>{closed}</div>
      </div>
      <div id={styles.settings}>
        <p id={styles.userName}>TechSolutions</p>
        <div id={styles.logoContainer}>
          <img src={CogWheel} id={styles.cogwheel} alt="NativeSoftware Logo" />
        </div>
        <div id={styles.logout} onClick={logout}>
          Logout
        </div>
        <div>

          {/* <Dropdown options={options} onChange={this._onSelect} value={defaultOption} placeholder="Select an option" />; */}
        </div>
      </div>
    </div>
  );
}
