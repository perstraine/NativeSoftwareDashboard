import axios from 'axios';
import { useEffect, useState } from "react";
import styles from "./PopupWindows.module.css";

function ViewResponseTime(props) {
    useEffect(() => {
        getInfo();
    }, [props.trigger]);

    const [supportLevel, setSupportLevel] = useState("");
    
    const userType = localStorage.getItem('userType');
    async function getInfo() {
        if(props.trigger){
            const url = process.env.REACT_APP_API_BASE_URL + "/api/CustomerView/Response";
            try {
                let userToken = localStorage.getItem("token");
                const config = { headers: { Authorization: `Bearer ${userToken}` },  params: { userType: userType }};
                const response = await axios.get(
                    url,config);
                    setSupportLevel(response.data);
                    //console.log(response.data);
            } 
            catch (error) {
                    console.log(error);
            }
        }
    }
    return (props.trigger)?(
        <div id={styles.popup} >
            <div id={styles.popupinner}>
                <div id={styles.userName}>
                    {userType}
                </div>
                <div  id={styles.windowName}>
                    <div>View Response Time</div>
                    <p>In hours</p>
                </div>
                <div>
                    <table id={styles.responseTable}>
                        <tbody>
                        <tr>
                            <td id={styles.tableHeader}></td>
                            <td id={styles.tableHeader}>Response Time</td>
                            <td id={styles.tableHeader}>Resolution Time</td>
                        </tr>
                        <tr>
                            <td id={styles.tableHeader}>Urgent</td>
                            <td>{supportLevel.responseTimeUrgent}</td>
                            <td>{supportLevel.resolutionTimeUrgent}</td>
                        </tr>
                        <tr>
                            <td id={styles.tableHeader}>High</td>
                            <td>{supportLevel.responseTimeHigh}</td>
                            <td>{supportLevel.resolutionTimeHigh}</td>
                        </tr>
                        <tr>
                            <td id={styles.tableHeader}>Normal</td>
                            <td>{supportLevel.responseTimeNormal}</td>
                            <td>{supportLevel.resolutionTimeNormal}</td>
                        </tr>
                        <tr>
                            <td id={styles.tableHeader}>Low</td>
                            <td>{supportLevel.responseTimeLow}</td>
                            <td>{supportLevel.resolutionTimeLow}</td>
                        </tr>
                        </tbody>
                    </table>
                </div>
            <button id={styles.closeButton} onClick={()=> props.setTrigger(false)}>Ok</button>
            </div>
        </div>):"";
}

export default ViewResponseTime


