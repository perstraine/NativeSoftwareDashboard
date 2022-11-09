import styles from "./JiraEpicSection.module.css";

export default function JiraEpicIssue({ epic }) {
    return (
      <div id={styles.epic} statusColor={epic.urgencyColour}>
        <div id={styles.account}>{epic.account}</div>
        <div id={styles.name}>{epic.name}</div>
        <div id={styles.start}>{epic.startDate.slice(0, 10)}</div>
        <div id={styles.due}>{epic.dueDate.slice(0, 10)}</div>
        <div id={styles.story}>{epic.storyPoints}</div>
        <div id={styles.budget}>{epic.budget}</div>
        <div id={styles.spent}>{epic.timeSpent}</div>
        <div id={styles.complete}>{`${epic.complete}%`}</div>
        <div id={styles.extra2}>
          <div className={styles.dot} statusColor={epic.urgencyColour}></div>
          <div className={styles.dot} statusColor={epic.urgencyColour}></div>
          <div className={styles.dot} statusColor={epic.urgencyColour}></div>
        </div>
      </div>
    );
}