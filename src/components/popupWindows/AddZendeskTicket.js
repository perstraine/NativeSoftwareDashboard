import axios from 'axios';
import { useEffect, useState, useRef } from "react";
import styles from "./PopupWindows.module.css";

function AddZendeskTicket(props) {
    useEffect(() => {
        getInfo();
      }, []);
    const[priorityOpen, setPriorityOpen] = useState(false);
    const[typeOpen, setTypeOpen] = useState(false);
    const[subject, setSubject] = useState("");
    const[comment, setComment] = useState("");
    const[priority, setPriority] = useState("");
    const[type, setType] = useState("");
    const [customer, setCustomer] = useState("");

    const handlePriorityOpen = () => {
        setPriorityOpen(!priorityOpen);
      };

    let customerEmail = localStorage.getItem('email');
    async function getInfo() {
      try {
        const response = await axios.get(
          "https://localhost:7001/api/DashboardInfo/Customer",{ params: { email: customerEmail } });
        //console.log(response);
        setCustomer(response.data.customer);
      } catch (error) {}
      }

    const onSubmitClick = (e) => {
        e.preventDefault();
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
                    "id": 12765904262169,
                    "value": true
                }
            ]
            }
          };
        console.log(ticket.ticket);
        axios.post('https://localhost:7001/api/NewTicket',ticket.ticket)
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
    return (props.trigger)?(
        <div id={styles.popup} >
            <div id={styles.popupinner}>
                <div id={styles.userName}>
                    {customer}
                </div>
                <div  id={styles.windowName}>
                    New Zendesk Ticket
                </div>
                <form id={styles.formStyle}>
                    <div id={styles.subject}>
                        <label>Subject:</label>
                        <input type="subject"
                        placeholder="Subject"
                        value={subject}
                        name="subject"
                        onChange={handleChange}></input>
                    </div>
                    <div id={styles.comment}>
                        <label>Comment:</label>
                        <input type="comment"
                        placeholder="Comment"
                        value={comment}
                        name="comment"
                        onChange={handleChange}></input>
                    </div>
                    <div id={styles.prioritytype} >
                        <div id={styles.prioritydropdown}>

                            {/* <button onClick={handlePriorityOpen}>Priority</button>
                            {priorityOpen ? (
                                <ul >
                                    <li>
                                        <button>Low</button>
                                    </li>
                                    <li>
                                        <button>Normal</button>
                                    </li>
                                    <li>
                                        <button>High</button>
                                    </li>
                                    <li>
                                        <button>Urgent</button>
                                    </li>
                                </ul>
                            ) : null}
                            {priorityOpen ? <div>Is Open</div> : <div>Is Closed</div>} */}

                            <label>Priority:</label>
                            <input type="priority"
                            placeholder="Priority"
                            value={priority}
                            name="priority"
                            onChange={handleChange}></input>

                        </div>
                        <div id={styles.typedropdown}>
                            <label>Type:</label>
                            <input type="type"
                            placeholder="Type"
                            value={type}
                            name="type"
                            onChange={handleChange}></input>
                        </div>
                      </div>
                </form>
                <div id={styles.submitbuttons}>
                <button id={styles.closeButton} onClick={()=> props.setTrigger(false)}>Cancel</button>
                <button id={styles.closeButton} onClick={onSubmitClick}>Ok</button>
                </div>
                {props.children}
            </div>
        </div>
    ):"";

}

export default AddZendeskTicket