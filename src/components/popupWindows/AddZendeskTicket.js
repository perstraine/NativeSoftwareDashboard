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

    let customerEmail = localStorage.getItem('email');
    async function getInfo() {
      try {
        const response = await axios.get(
          "https://localhost:7001/api/DashboardInfo/Customer",{ params: { email: customerEmail } });
        console.log(response);
        setCustomer(response.data.customer);
      } catch (error) {}
      }


    // let menuRef = useRef();
    // useEffect(()=>{
    //   let handler = (e)=>{
    //     if(!menuRef.current.contains(e.target)){
    //         setPriorityOpen(false);
    //     }
    //   };
    //   document.addEventListener("mousedown", handler);
    // });

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


{/* <div id={styles.menuContainer}>
<div id={styles.menuTrigger}>
    <button  onClick={()=>{setPriorityOpen(true)}}>{priority}</button>
</div>
{priorityOpen?
    <div id = {styles.dropdownMenu}>
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
</div>

<div id={styles.menuContainer}>
<div id={styles.menuTrigger} onClick={()=>{setTypeOpen(true)}}>
    <button>{type}</button>
</div>
{priorityOpen?
<div id = {styles.dropdownMenu}>
    <ul>
        <li className={styles.dropdownItem} onClick={()=>setType("question")}><h3>Question</h3></li>
        <li className={styles.dropdownItem} onClick={()=>setType("incident")}><h3>Incident</h3></li>
        <li className={styles.dropdownItem} onClick={()=>setType("problem")}><h3>Problem</h3></li>
        <li className={styles.dropdownItem} onClick={()=>setType("task")}><h3>Task</h3></li>
    </ul>
</div>
:
null
}
</div> */}