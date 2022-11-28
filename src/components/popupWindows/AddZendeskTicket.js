import axios from 'axios';
import {useState} from "react";
import styles from "./PopupWindows.module.css";


function AddZendeskTicket(props) {
    const[subject, setSubject] = useState("");
    const[comment, setComment] = useState("");
    const[priority, setPriority] = useState("");
    const[type, setType] = useState("");
    

    const onSubmitClick = (e) => {
        e.preventDefault();
        setSubject(subject);
        setComment(comment);
        setPriority(priority);
        setType(type);
        const ticket = 
        {
            "ticket": {
              "comment": {
                "body": comment
              },
              "priority": priority,
              "subject": subject,
              "type": type,
              "custom_fields": [
                {
                    "id": 12765904262169,
                    "value": true
                }
            ]
            }
          };
      console.log(ticket.ticket);
      const url = process.env.REACT_APP_API_BASE_URL + "/api/NewTicket";
        axios.post(url,ticket)
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
          <div id={styles.userName}>TechSolutions</div>
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
              {/* <div id={styles.menuContainer} ref={menuRef}>
                            <div id={styles.menuTrigger}>
                                <button  onClick={()=>{setPriorityOpen(true)}}>{priority}</button>
                            </div>
                            {priorityOpen?
                            <div id = {styles.dropdownMenuPriority}>
                                <ul>
                                    <li className={styles.dropdownItem} onClick={()=>setPriority("low")}><h3>Low</h3></li>
                                    <li className={styles.dropdownItem} onClick={()=>setPriority("normal")}><h3>Normal</h3></li>
                                    <li className={styles.dropdownItem} onClick={()=>setPriority("high")}><h3>High</h3></li>
                                    <li className={styles.dropdownItem} onClick={()=>setPriority("urgent")}><h3>Urgent</h3></li>
                                </ul>
                            </div>
                            :
                            null
                            }
                        </div> */}
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
                  <option value="normal">Normal</option>
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
                  <option value="question">Question</option>
                  <option value="incident">Incident</option>
                  <option value="problem">Problem</option>
                  <option value="task">Task</option>
                </select>
              </div>
            </div>
          </form>
          <div id={styles.submitbuttons}>
            <button
              id={styles.closeButton}
              onClick={() => props.setTrigger(false)}
            >
              Cancel
            </button>
            <button id={styles.closeButton} onClick={onSubmitClick}>
              Ok
            </button>
          </div>
          {props.children}
        </div>
      </div>
    ) : (
      ""
    );

}

export default AddZendeskTicket