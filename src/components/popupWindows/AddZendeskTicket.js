import React from 'react'
import styles from "./PopupWindows.module.css";


function AddZendeskTicket(props) {
  return (props.trigger)?(
    <div id={styles.popup} >
        <div id={styles.popupinner}>
            Add Zendesk Ticket
            <button id={styles.closeButton} onClick={()=> props.setTrigger(false)}>OK</button>
            {props.children}
        </div>
    </div>
  ):"";

}

export default AddZendeskTicket