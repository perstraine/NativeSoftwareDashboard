import styles from "./CustomerTicket.module.css"
import {useState, useEffect, useRef} from "react";

function CustomerTicket(props) {
  const [isOpen, setIsOpen] = useState(false);
  let refUrl = useRef();
  useEffect(() => {
    let handler = (event) =>{
      if(!refUrl.current.contains(event.target)){
        setIsOpen(false);
      }
    }
    document.addEventListener("mousedown", handler);
    return() => {
      document.removeEventListener("mousedown", handler);
    }
  })
  function handleClick() {
    setIsOpen(!isOpen);
  }

  return (
    <div status-bg-colour={props.ticket.trafficLight} className={styles.ticket}>
      
      <p status-text-colour={props.ticket.trafficLight} className={styles.ticketText} id={styles.sub}> {props.ticket.subject}</p>
      <p status-text-colour={props.ticket.trafficLight} className={styles.ticketText} id={styles.reqTime}> {props.ticket.requestedDate}</p>
      <p status-text-colour={props.ticket.trafficLight} className={styles.ticketText} id={styles.firstRes}> {props.ticket.firstResponse}</p>
      <p status-text-colour={props.ticket.trafficLight} className={styles.ticketText} id={styles.lastUpdate}> {props.ticket.lastUpdate}</p>
      <p status-text-colour={props.ticket.trafficLight} className={styles.ticketText} id={styles.due}> {props.ticket.timeDue}</p>
      <p status-text-colour={props.ticket.trafficLight} className={styles.ticketText} id={styles.priority}> {props.ticket.priority}</p>
      <p status-text-colour={props.ticket.trafficLight} className={styles.ticketText} id={styles.type}> {props.ticket.type ? props.ticket.type :"Null" }</p>
      <div className = {styles.externalLinks} id={styles.links} onClick={handleClick} ref={refUrl}>
        <div status-dot-colour={props.ticket.trafficLight} className = {styles.dot}></div>
        <div status-dot-colour={props.ticket.trafficLight} className = {styles.dot}></div>
        <div status-dot-colour={props.ticket.trafficLight} className = {styles.dot}></div>
        {isOpen && <div id={styles.dropdown}><a href={props.ticket.url} target="_blank" className={styles.dropdownItem}>View in Zendesk</a></div>}
      </div>
    </div>
  )
}

export default CustomerTicket