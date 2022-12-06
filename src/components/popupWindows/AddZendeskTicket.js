import axios from 'axios';
import {useState, useEffect} from "react";
import styles from "./PopupWindows.module.css";

function AddZendeskTicket(props) {
    useEffect(() => {
        getInfo();
      }, []);
    const[disableButton, setDisableButton] = useState(false);
    const[subject, setSubject] = useState("");
    const[comment, setComment] = useState("");
    const[priority, setPriority] = useState("normal");
    const[type, setType] = useState("question");
    const[customer, setCustomer] = useState("");

    let customerEmail = localStorage.getItem('email');
    async function getInfo() {
      try {
        let userToken = localStorage.getItem("token");
        let customerEmail = localStorage.getItem('email');
        const url = process.env.REACT_APP_API_BASE_URL + "/api/DashboardInfo/Customer";
        const config = { headers: { Authorization: `Bearer ${userToken}` },  params: { email: customerEmail }};
        const response = await axios.get(
          url,config);
        //console.log(response);
        setCustomer(response.data.customer);
      } catch (error) {}
      }

      function disableOkButton(){
        onSubmitClick();
        
      }
      const onSubmitClick = () => {
        setDisableButton(true);
        setSubject(subject);
        setComment(comment);
        setPriority(priority);
        setType(type);
        const ticket = 
        {
            "ticket": {
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
            }
          };
      console.log(ticket.ticket);
      const url = process.env.REACT_APP_API_BASE_URL + "/api/NewTicket";
        axios.post(url,ticket.ticket)
        .then((response) => {
            props.setTrigger(false);
            //console.log(response);
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