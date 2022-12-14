import axios from 'axios';
import {useState, useEffect} from "react";
import styles from "./PopupWindows.module.css";

function AddZendeskTicket(props) {
const BASE_URL = window.BASE_URL;  
useEffect(() => {
      getInfo();
    }, []);
  const[disableButton, setDisableButton] = useState(false);
  const[subject, setSubject] = useState("");
  const[comment, setComment] = useState("");
  const[priority, setPriority] = useState("normal");
  const[type, setType] = useState("question");
  const[customer, setCustomer] = useState("");

  const customerEmail = localStorage.getItem('email');
  const userType = localStorage.getItem('userType');
  async function getInfo() {
    try {
      const url = BASE_URL + "/api/DashboardInfo/Customer";

      const response = await axios.get(
        url,{ params: { email: customerEmail } });
      //console.log(response);
      setCustomer(response.data.customer);
    } catch (error) {}
  }

    const onSubmitClick = () => {
      console.log('clicked');
      setDisableButton(true);
      setSubject(subject);
      setComment(comment);
      setPriority(priority);
      setType(type);
      var data = JSON.stringify({
        "description": comment,
        "priority": priority,
        "subject": subject,
        "type": type,
        "email": localStorage.getItem('email'),
        "custom_fields": [
          {
            "value": true
          }
        ]
      });

    const userToken = localStorage.getItem("token");
    const config = {
      method: 'post',
      url : BASE_URL + "/api/NewTicket",
      headers: { 
        'Authorization': `Bearer ${userToken}`, 
        'Content-Type': 'application/json'
        },
      data : data
    };
      axios(config)
      .then((response) => {
          props.setTrigger(false);
          console.log(response);
      })
      .catch((error) => {
        console.log('Error',error);
          console.log(error);
      });
  }

  function handleChange(e) {
      if (e.target.name === 'subject') {
          setSubject(e.target.value)
      } 
      if (e.target.name === 'comment') {
          setComment(e.target.value)
      } 
      if (e.target.name === 'priority') {
          setPriority(e.target.value)
      } 
      if (e.target.name === 'type') {
          setType(e.target.value)
      } 
  }
  return props.trigger ? (
    <div id={styles.popup}>
      <div id={styles.popupinner}>
        <div id={styles.userName}>{customer}</div>
        <div id={styles.userName}>
                    {userType}
                </div>
        <div id={styles.windowName}>New Zendesk Ticket</div>
        <form id={styles.formStyle}>
          <div id={styles.subject}>
            <label className={styles.label}>Subject:</label>
            <input
              className={styles.input}
              type="subject"
              placeholder="Subject"
              value={subject}
              name="subject"
              onChange={handleChange}
            ></input>
          </div>
          <div id={styles.comment}>
            <label className={styles.label}>Comment:</label>
            <input
              className={styles.input}
              type="comment"
              placeholder="Comment"
              value={comment}
              name="comment"
              onChange={handleChange}
            ></input>
          </div>
          <div id={styles.prioritytype}>
            <div id={styles.prioritydropdown}>
              <label className={styles.label}>Priority:</label>
              <select
                className={styles.input}
                type="priority"
                placeholder="Priority"
                value={priority}
                name="priority"
                onChange={handleChange}
              >
                <option value="urgent">Urgent</option>
                <option value="high">High</option>
                <option value="normal" selected>
                  Normal
                </option>
                <option value="low">Low</option>
              </select>
            </div>
            <div id={styles.typedropdown}>
              <label className={styles.label}>Type:</label>
              <select
                className={styles.input}
                type="type"
                placeholder="Type"
                value={type}
                name="type"
                onChange={handleChange}
              >
                <option value="question" selected>
                  Question
                </option>
                <option value="incident">Incident</option>
                <option value="problem">Problem</option>
                <option value="task">Task</option>
              </select>
            </div>
          </div>
        </form>
        <div id={styles.submitbuttons}>
          <button id={styles.closeButton} onClick={() => props.setTrigger(false)}> Cancel </button>
          <button
            id={styles.closeButton}
            onClick={() => {
              if (!disableButton && comment!=='' && subject !== '') {
                setDisableButton(true);
                onSubmitClick();
              }
            }}
          > Ok </button>
        </div>
        {props.children}
      </div>
    </div>
  ) : (
    ""
  );

}

export default AddZendeskTicket